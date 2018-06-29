DELIMITER $$

DROP TEMPORARY TABLE IF EXISTS ki_upgrade_log $$
CREATE TEMPORARY TABLE ki_upgrade_log
(message VARCHAR(200)) $$

DROP FUNCTION IF EXISTS FNC_TABLE_EXIST $$
CREATE FUNCTION FNC_TABLE_EXIST(Name VARCHAR(1000)) RETURNS BOOLEAN
BEGIN
	IF EXISTS 
		(
			SELECT 1 FROM information_schema.TABLES 
            WHERE table_name = Name AND table_schema = DATABASE() 
            AND table_type = 'BASE TABLE'
		) THEN
        RETURN TRUE;
	ELSE
		RETURN FALSE;
	END IF;
END $$

DROP PROCEDURE IF EXISTS RENAME_TABLE $$
CREATE PROCEDURE RENAME_TABLE(oldname VARCHAR(1000), newname VARCHAR(1000))
BEGIN
	SET @query = CONCAT('RENAME TABLE ', oldname, ' TO ', newname);
	PREPARE stmt FROM @query;
	EXECUTE stmt;
END $$

DROP FUNCTION IF EXISTS GET_KI_DB_VERSION $$
CREATE FUNCTION GET_KI_DB_VERSION() RETURNS VARCHAR(128)
BEGIN
	DECLARE Version VARCHAR(128);
	IF FNC_TABLE_EXIST('meta') THEN
		SET Version = (SELECT version_guid FROM meta WHERE meta_id = 1);
	ELSE
		IF NOT FNC_TABLE_EXIST('backup_gameevents_log') THEN
			INSERT INTO ki_upgrade_log VALUES ("Detected Database Version is 0.76 or earlier");
			SET Version = "9ff4a3e7-f66c-4894-af05-3a982612e2cc";
        ELSE
			INSERT INTO ki_upgrade_log VALUES ("Detected Database Version is 0.77 or earlier");
			SET Version = "39124186-cc9e-49c0-a23c-d68c8fe9ad81";
        END IF;
    END IF;
    
    IF Version IS NULL THEN
		INSERT INTO ki_upgrade_log VALUES ("Unable To Detect Database Version - Version Is Corrupted!");
		SET Version = "e6726cbe-c673-4724-a456-f5863735cb4d";
    END IF;
    
    RETURN Version;
END $$

DROP PROCEDURE IF EXISTS UPGRADE_KI_DB $$
CREATE PROCEDURE UPGRADE_KI_DB(Version VARCHAR(128))
BEGIN
  INSERT INTO ki_upgrade_log VALUES ("Starting Upgrade");
  
  IF Version = "e6726cbe-c673-4724-a456-f5863735cb4d" THEN
	SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Cannot Upgrade Database! Corrupted Version Found!';
  END IF;
	
  -- VERSION 0.76 and below
  IF Version = "9ff4a3e7-f66c-4894-af05-3a982612e2cc" THEN
	INSERT INTO ki_upgrade_log VALUES ("Database Version is 0.76 or earlier - Upgrading to 0.77");
    
	CREATE TABLE IF NOT EXISTS backup_gameevents_log LIKE raw_gameevents_log;
    
    INSERT INTO ki_upgrade_log VALUES ("Database Upgraded To Version 0.77");
    SET Version = "39124186-cc9e-49c0-a23c-d68c8fe9ad81";
  END IF;
  
  -- VERSION 0.77
  IF Version = "39124186-cc9e-49c0-a23c-d68c8fe9ad81" THEN
	INSERT INTO ki_upgrade_log VALUES ("Database Version is 0.77 - Upgrading to 0.78");
    
	-- drop table rpt_updated
	DROP TABLE IF EXISTS rpt_updated;
    
    -- create table meta
    CREATE TABLE IF NOT EXISTS meta 
    (
	  meta_id INT NOT NULL,
	  version VARCHAR(45) NOT NULL,
	  version_guid VARCHAR(128) NOT NULL,
	  rpt_last_updated DATETIME NULL,
	  PRIMARY KEY (meta_id)
	);
    
    -- insert data
    INSERT INTO meta (meta_id, version, version_guid, rpt_last_updated)
    VALUES (1, "0.78", "0bc9fc46-ea51-4fc4-9c52-d3793c9a4515", NULL);
    
    INSERT INTO ki_upgrade_log VALUES ("Database Upgraded To Version 0.78");
    SET Version = "0bc9fc46-ea51-4fc4-9c52-d3793c9a4515";
  END IF;
  
  -- VERSION 0.78
  IF Version = "0bc9fc46-ea51-4fc4-9c52-d3793c9a4515" THEN
	INSERT INTO ki_upgrade_log VALUES ("Database Version is 0.78 - Upgrading to 0.79");
  
	ALTER TABLE server
	ADD COLUMN `description` VARCHAR(900) NULL COMMENT 'server description displayed on website' AFTER `name`;
  
	-- insert data
    INSERT INTO meta (meta_id, version, version_guid, rpt_last_updated)
    VALUES (1, "0.79", "5762581d943af07f9b4864e30e9070e3305b5b77", NULL);
    
    INSERT INTO ki_upgrade_log VALUES ("Database Upgraded To Version 0.79");
	SET Version = "5762581d943af07f9b4864e30e9070e3305b5b77";
  END IF;
  
  -- VERSION 0.79
  IF Version = "5762581d943af07f9b4864e30e9070e3305b5b77" THEN
    INSERT INTO ki_upgrade_log VALUES ("Database Version is 0.79 - Upgrading to 0.80");
  
	-- Custom Menu Items that can be dynamically added to the Game Navigation bar	
	CREATE TABLE IF NOT EXISTS custom_menu_item (
	  `custom_menu_item_id` int(11) NOT NULL AUTO_INCREMENT,
	  `server_id` int(11) NOT NULL,
	  `menu_name` varchar(30) NOT NULL,
	  `icon_class` varchar(45) NOT NULL,
	  `html_content` varchar(300) NOT NULL,
	  PRIMARY KEY (`custom_menu_item_id`)
	) ENGINE=InnoDB DEFAULT CHARSET=utf8;
	  PRIMARY KEY (`custom_menu_item_id`),
	  KEY `fk_server_id_idx` (`server_id`),
	  CONSTRAINT `fk_custom_menu_item_server_id` FOREIGN KEY (`server_id`) REFERENCES `server` (`server_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
	) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
	  
	-- Simple Radio support and integration
	ALTER TABLE server
	ADD COLUMN `simple_radio_enabled` BIT(1) NULL DEFAULT 0 AFTER `ip_address`,
	ADD COLUMN `simple_radio_ip_address` VARCHAR(40) NULL AFTER `simple_radio_enabled`,
	ADD COLUMN `map_id` INT(11) NULL AFTER `server_id`;
	
	ALTER TABLE server
	CHANGE COLUMN `description` `description` VARCHAR(900) NOT NULL COMMENT 'server description displayed on website' ,
	CHANGE COLUMN `simple_radio_enabled` `simple_radio_enabled` BIT(1) NOT NULL DEFAULT b'0' ,
	CHANGE COLUMN `simple_radio_ip_address` `simple_radio_ip_address` VARCHAR(40) NOT NULL ;
	
	-- MapBox support was replaced with google maps - map data is no longer stored in DB
	DROP TABLE IF EXISTS xref_game_map_server;
	DROP TABLE IF EXISTS map_layer;
	DROP TABLE IF EXISTS game_map;
	
	-- This data was moved to Redis, no longer stored in MySql
	DROP TABLE IF EXISTS capture_point;
	DROP TABLE IF EXISTS depot;
	DROP TABLE IF EXISTS online_players;
	DROP TABLE IF EXISTS side_mission;
	
	-- DCS Map Names table 
	CREATE TABLE IF NOT EXISTS map (
	  `map_id` int(11) NOT NULL AUTO_INCREMENT,
	  `name` varchar(30) NOT NULL,
	  PRIMARY KEY (`map_id`),
	  UNIQUE KEY `map_id_UNIQUE` (`map_id`)
	) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
	
	-- Re Added Online Players but in a different scheme
	CREATE TABLE IF NOT EXISTS online_players (
	  `server_id` int(11) NOT NULL,
	  `ucid` varchar(128) NOT NULL,
	  UNIQUE KEY `ucid_UNIQUE` (`ucid`),
	  KEY `fk_server_id_idx` (`server_id`),
	  CONSTRAINT `fk_server_id` FOREIGN KEY (`server_id`) REFERENCES `server` (`server_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
	  CONSTRAINT `fk_ucid` FOREIGN KEY (`ucid`) REFERENCES `player` (`ucid`) ON DELETE NO ACTION ON UPDATE NO ACTION
	) ENGINE=InnoDB DEFAULT CHARSET=utf8;

  
	-- insert data
    INSERT INTO meta (meta_id, version, version_guid, rpt_last_updated)
    VALUES (1, "0.80", "4be0bd54461842c23a1da692ec236a1f736f70cc", NULL);
    
    INSERT INTO ki_upgrade_log VALUES ("Database Upgraded To Version 0.80");
	SET Version = "4be0bd54461842c23a1da692ec236a1f736f70cc";
  END IF;
  
  -- VERSION 0.80
  IF Version = "4be0bd54461842c23a1da692ec236a1f736f70cc" THEN
    INSERT INTO ki_upgrade_log VALUES ("Database Version is 0.80 - Upgrading to 0.90");
	
	-- Added time stamp columns to connection log tables
	ALTER TABLE `raw_connection_log` 
	ADD COLUMN `time` DATETIME(0) NOT NULL AFTER `real_time`;

	ALTER TABLE `backup_connection_log` 
	ADD COLUMN `time` DATETIME(0) NOT NULL AFTER `real_time`;
	
	-- Update all date stamps with a default value of 2018-01-01 12:00:00
	UPDATE raw_connection_log 
	SET time = '2018-01-01 12:00:00'
	WHERE time IS NULL OR time = '0000-00-00 00:00:00';
	
	UPDATE backup_connection_log 
	SET time = '2018-01-01 12:00:00'
	WHERE time IS NULL OR time = '0000-00-00 00:00:00';
	
	-- Added trigger to auto set the current date/time on new records
	CREATE TRIGGER `trg_raw_connection_log_current_time` BEFORE INSERT ON  `raw_connection_log` 
	FOR EACH ROW 
	SET NEW.time = NOW();
	
	-- Added new reporting table for player online activity
	CREATE TABLE `rpt_player_online_activity` (
	  `id` BIGINT(32) NOT NULL AUTO_INCREMENT,
	  `ucid` VARCHAR(128) NOT NULL,
	  `date` DATE NOT NULL,
	  `total_game_time` BIGINT(32) NOT NULL DEFAULT 0,
	  PRIMARY KEY (`id`));
	  
	-- Added date stamp columns to gameevents log tables
	ALTER TABLE `raw_gameevents_log` 
	ADD COLUMN `date` DATE NOT NULL AFTER `ucid`;
	
	ALTER TABLE `backup_gameevents_log` 
	ADD COLUMN `date` DATE NOT NULL AFTER `ucid`;
	
	-- Added trigger to auto insert the current date on new records
	CREATE TRIGGER `trg_raw_gameevents_log_current_time` BEFORE INSERT ON  `raw_gameevents_log` 
	FOR EACH ROW 
	SET NEW.date = CURDATE();

	-- Update all date stamps with a default value of 2018-01-01
	UPDATE raw_gameevents_log 
	SET date = '2018-01-01'
	WHERE date IS NULL OR date = '0000-00-00';
	
	UPDATE backup_gameevents_log 
	SET date = '2018-01-01'
	WHERE date IS NULL OR date = '0000-00-00';
	
	-- Added new reporting table for sorties over time 
	CREATE TABLE `rpt_sorties_over_time` (
	  `id` BIGINT(32) NOT NULL AUTO_INCREMENT,
	  `ucid` VARCHAR(128) NOT NULL,
	  `airframe` VARCHAR(45) NOT NULL,
	  `date` DATE NOT NULL,
	  `sorties` INT NOT NULL DEFAULT 0,
	  `kills` INT NOT NULL DEFAULT 0,
	  `deaths` INT NOT NULL DEFAULT 0,
	  `slingloads` INT NOT NULL DEFAULT 0,
	  `transport` INT NOT NULL DEFAULT 0,
	  `resupplies` INT NOT NULL DEFAULT 0,
	  PRIMARY KEY (`id`));
	
	-- insert data
    INSERT INTO meta (meta_id, version, version_guid, rpt_last_updated)
    VALUES (1, "0.90", "--------", NULL);
    
    INSERT INTO ki_upgrade_log VALUES ("Database Upgraded To Version 0.90");
	SET Version = ""--------",";
  END IF;
  
  SELECT * FROM ki_upgrade_log;

END $$

-- SELECT "Created Upgrade Code - Preparing To Upgrade Database";

CALL UPGRADE_KI_DB(GET_KI_DB_VERSION()) $$

DROP PROCEDURE IF EXISTS UPGRADE_KI_DB $$
DROP FUNCTION IF EXISTS FNC_TABLE_EXIST $$
DROP PROCEDURE IF EXISTS RENAME_TABLE $$


DROP TEMPORARY TABLE IF EXISTS ki_upgrade_log;
 
DELIMITER ;

