CREATE DATABASE  IF NOT EXISTS `ki` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ki`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: ki
-- ------------------------------------------------------
-- Server version	5.7.20-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `backup_connection_log`
--

DROP TABLE IF EXISTS `backup_connection_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `backup_connection_log` (
  `id` bigint(32) unsigned NOT NULL AUTO_INCREMENT,
  `server_id` int(11) NOT NULL,
  `session_id` int(11) NOT NULL,
  `type` varchar(20) NOT NULL,
  `player_ucid` varchar(128) NOT NULL,
  `player_name` varchar(128) NOT NULL,
  `player_id` int(11) NOT NULL,
  `ip_address` varchar(20) NOT NULL,
  `game_time` bigint(32) NOT NULL,
  `real_time` bigint(32) NOT NULL,
  `time` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=584 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `backup_gameevents_log`
--

DROP TABLE IF EXISTS `backup_gameevents_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `backup_gameevents_log` (
  `id` bigint(32) NOT NULL AUTO_INCREMENT,
  `server_id` int(11) NOT NULL,
  `session_id` bigint(32) NOT NULL,
  `sortie_id` bigint(32) DEFAULT NULL,
  `ucid` varchar(128) DEFAULT NULL,
  `date` date NOT NULL,
  `event` varchar(45) NOT NULL,
  `player_name` varchar(128) NOT NULL,
  `player_side` int(11) DEFAULT NULL,
  `model_time` bigint(32) NOT NULL,
  `game_time` bigint(32) NOT NULL,
  `role` varchar(45) DEFAULT NULL,
  `location` varchar(60) DEFAULT NULL,
  `weapon` varchar(60) DEFAULT NULL,
  `weapon_category` varchar(20) DEFAULT NULL,
  `target_name` varchar(60) DEFAULT NULL,
  `target_model` varchar(60) DEFAULT NULL,
  `target_type` varchar(25) DEFAULT NULL,
  `target_category` varchar(15) DEFAULT NULL,
  `target_side` int(11) DEFAULT NULL,
  `target_is_player` bit(1) DEFAULT NULL,
  `target_player_ucid` varchar(128) DEFAULT NULL,
  `target_player_name` varchar(128) DEFAULT NULL,
  `transport_unloaded_count` int(11) DEFAULT NULL,
  `cargo` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2023 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `custom_menu_item`
--

DROP TABLE IF EXISTS `custom_menu_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `custom_menu_item` (
  `custom_menu_item_id` int(11) NOT NULL AUTO_INCREMENT,
  `server_id` int(11) NOT NULL,
  `menu_name` varchar(30) NOT NULL,
  `icon_class` varchar(45) NOT NULL,
  `html_content` varchar(300) NOT NULL,
  PRIMARY KEY (`custom_menu_item_id`),
  KEY `fk_server_id_idx` (`server_id`),
  CONSTRAINT `fk_custom_menu_item_server_id` FOREIGN KEY (`server_id`) REFERENCES `server` (`server_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `map`
--

DROP TABLE IF EXISTS `map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `map` (
  `map_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(30) NOT NULL,
  PRIMARY KEY (`map_id`),
  UNIQUE KEY `map_id_UNIQUE` (`map_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `meta`
--

DROP TABLE IF EXISTS `meta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `meta` (
  `meta_id` int(11) NOT NULL,
  `version` varchar(45) NOT NULL,
  `version_guid` varchar(128) NOT NULL,
  `rpt_last_updated` datetime DEFAULT NULL,
  PRIMARY KEY (`meta_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `online_players`
--

DROP TABLE IF EXISTS `online_players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `online_players` (
  `server_id` int(11) NOT NULL,
  `ucid` varchar(128) NOT NULL,
  UNIQUE KEY `ucid_UNIQUE` (`ucid`),
  KEY `fk_server_id_idx` (`server_id`),
  CONSTRAINT `fk_server_id` FOREIGN KEY (`server_id`) REFERENCES `server` (`server_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_ucid` FOREIGN KEY (`ucid`) REFERENCES `player` (`ucid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `player`
--

DROP TABLE IF EXISTS `player`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `player` (
  `ucid` varchar(128) NOT NULL,
  `name` varchar(128) NOT NULL,
  `lives` int(11) NOT NULL,
  `banned` bit(1) NOT NULL,
  PRIMARY KEY (`ucid`),
  UNIQUE KEY `ucid_UNIQUE` (`ucid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `raw_connection_log`
--

DROP TABLE IF EXISTS `raw_connection_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `raw_connection_log` (
  `id` bigint(32) unsigned NOT NULL AUTO_INCREMENT,
  `server_id` int(11) NOT NULL,
  `session_id` int(11) NOT NULL,
  `type` varchar(20) NOT NULL,
  `player_ucid` varchar(128) NOT NULL,
  `player_name` varchar(128) NOT NULL,
  `player_id` int(11) NOT NULL,
  `ip_address` varchar(20) NOT NULL,
  `game_time` bigint(32) NOT NULL,
  `real_time` bigint(32) NOT NULL,
  `time` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `trg_raw_connection_log_current_time` BEFORE INSERT ON  `raw_connection_log` 
FOR EACH ROW 
SET NEW.time = NOW() */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `raw_gameevents_log`
--

DROP TABLE IF EXISTS `raw_gameevents_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `raw_gameevents_log` (
  `id` bigint(32) NOT NULL AUTO_INCREMENT,
  `server_id` int(11) NOT NULL,
  `session_id` bigint(32) NOT NULL,
  `sortie_id` bigint(32) DEFAULT NULL,
  `ucid` varchar(128) DEFAULT NULL,
  `date` date NOT NULL,
  `event` varchar(45) NOT NULL,
  `player_name` varchar(128) NOT NULL,
  `player_side` int(11) DEFAULT NULL,
  `model_time` bigint(32) NOT NULL,
  `game_time` bigint(32) NOT NULL,
  `role` varchar(45) DEFAULT NULL,
  `location` varchar(60) DEFAULT NULL,
  `weapon` varchar(60) DEFAULT NULL,
  `weapon_category` varchar(20) DEFAULT NULL,
  `target_name` varchar(60) DEFAULT NULL,
  `target_model` varchar(60) DEFAULT NULL,
  `target_type` varchar(25) DEFAULT NULL,
  `target_category` varchar(15) DEFAULT NULL,
  `target_side` int(11) DEFAULT NULL,
  `target_is_player` bit(1) DEFAULT NULL,
  `target_player_ucid` varchar(128) DEFAULT NULL,
  `target_player_name` varchar(128) DEFAULT NULL,
  `transport_unloaded_count` int(11) DEFAULT NULL,
  `cargo` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `trg_raw_gameevents_log_current_time` BEFORE INSERT ON  `raw_gameevents_log` 
FOR EACH ROW 
SET NEW.date = CURDATE() */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `role_image`
--

DROP TABLE IF EXISTS `role_image`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `role_image` (
  `role_image_id` int(11) NOT NULL AUTO_INCREMENT,
  `image` varchar(132) NOT NULL,
  `role` varchar(45) NOT NULL,
  PRIMARY KEY (`role_image_id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_airframe_kd`
--

DROP TABLE IF EXISTS `rpt_airframe_kd`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_airframe_kd` (
  `ucid` varchar(128) NOT NULL,
  `airframe` varchar(45) NOT NULL,
  `name` varchar(60) NOT NULL,
  `kills` int(11) NOT NULL DEFAULT '0',
  `deaths` int(11) NOT NULL DEFAULT '0',
  `is_model` bit(1) NOT NULL DEFAULT b'0',
  `is_type` bit(1) NOT NULL DEFAULT b'0',
  `is_category` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_airframe_sortie`
--

DROP TABLE IF EXISTS `rpt_airframe_sortie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_airframe_sortie` (
  `sortie_id` int(11) NOT NULL AUTO_INCREMENT,
  `ucid` varchar(128) NOT NULL,
  `airframe` varchar(45) NOT NULL,
  `sortie_time` bigint(64) NOT NULL,
  `shots` int(11) NOT NULL,
  `gunshots` int(11) NOT NULL,
  `hits` int(11) NOT NULL,
  `kills` int(11) NOT NULL,
  `transport_mount` int(11) NOT NULL,
  `transport_dismount` int(11) NOT NULL,
  `cargo_unpacked` int(11) NOT NULL,
  `depot_resupply` int(11) NOT NULL,
  `cargo_hooked` int(11) NOT NULL,
  `cargo_unhooked` int(11) NOT NULL,
  `hits_received` int(11) NOT NULL,
  PRIMARY KEY (`sortie_id`)
) ENGINE=InnoDB AUTO_INCREMENT=155 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_airframe_stats`
--

DROP TABLE IF EXISTS `rpt_airframe_stats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_airframe_stats` (
  `ucid` varchar(128) NOT NULL,
  `airframe` varchar(45) NOT NULL,
  `flight_time` bigint(64) NOT NULL DEFAULT '0',
  `avg_flight_time` bigint(64) NOT NULL DEFAULT '0',
  `takeoffs` int(11) NOT NULL DEFAULT '0',
  `landings` int(11) NOT NULL DEFAULT '0',
  `slingload_hooks` int(11) NOT NULL DEFAULT '0',
  `slingload_unhooks` int(11) NOT NULL DEFAULT '0',
  `kills` int(11) NOT NULL DEFAULT '0',
  `kills_player` int(11) NOT NULL DEFAULT '0',
  `kills_friendly` int(11) NOT NULL DEFAULT '0',
  `deaths` int(11) NOT NULL DEFAULT '0',
  `transport_mounts` int(11) NOT NULL DEFAULT '0',
  `transport_dismounts` int(11) NOT NULL DEFAULT '0',
  `depot_resupplies` int(11) NOT NULL DEFAULT '0',
  `cargo_unpacked` int(11) NOT NULL DEFAULT '0',
  `ejects` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_airframe_weapon`
--

DROP TABLE IF EXISTS `rpt_airframe_weapon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_airframe_weapon` (
  `ucid` varchar(128) NOT NULL,
  `airframe` varchar(45) NOT NULL,
  `weapon` varchar(60) NOT NULL,
  `shots` int(11) NOT NULL DEFAULT '0',
  `hits` int(11) NOT NULL DEFAULT '0',
  `kills` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_overall_server_traffic`
--

DROP TABLE IF EXISTS `rpt_overall_server_traffic`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_overall_server_traffic` (
  `server_id` int(11) NOT NULL,
  `ucid` varchar(128) NOT NULL,
  `num_connects` int(11) NOT NULL DEFAULT '0',
  `avg_online_time` bigint(64) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_overall_stats`
--

DROP TABLE IF EXISTS `rpt_overall_stats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_overall_stats` (
  `ucid` varchar(128) NOT NULL,
  `game_time` bigint(64) NOT NULL DEFAULT '0' COMMENT 'total in game time in seconds',
  `takeoffs` int(11) NOT NULL DEFAULT '0',
  `landings` int(11) NOT NULL DEFAULT '0',
  `slingload_hooks` int(11) NOT NULL DEFAULT '0',
  `slingload_unhooks` int(11) NOT NULL DEFAULT '0',
  `kills` int(11) NOT NULL DEFAULT '0',
  `deaths` int(11) NOT NULL DEFAULT '0',
  `ejects` int(11) NOT NULL DEFAULT '0',
  `transport_mounts` int(11) NOT NULL DEFAULT '0',
  `transport_dismounts` int(11) NOT NULL DEFAULT '0',
  `depot_resupplies` int(11) NOT NULL DEFAULT '0',
  `cargo_unpacked` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ucid`),
  UNIQUE KEY `ucid_UNIQUE` (`ucid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_player_online_activity`
--

DROP TABLE IF EXISTS `rpt_player_online_activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_player_online_activity` (
  `id` bigint(32) NOT NULL AUTO_INCREMENT,
  `ucid` varchar(128) NOT NULL,
  `date` date NOT NULL,
  `total_game_time` bigint(32) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_player_session_series`
--

DROP TABLE IF EXISTS `rpt_player_session_series`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_player_session_series` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ucid` varchar(128) NOT NULL,
  `session_id` bigint(32) NOT NULL,
  `event` varchar(45) NOT NULL,
  `game_time` bigint(32) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_session_idx` (`session_id`),
  CONSTRAINT `fk_session` FOREIGN KEY (`session_id`) REFERENCES `session` (`session_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1303 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpt_sorties_over_time`
--

DROP TABLE IF EXISTS `rpt_sorties_over_time`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpt_sorties_over_time` (
  `id` bigint(32) NOT NULL AUTO_INCREMENT,
  `ucid` varchar(128) NOT NULL,
  `airframe` varchar(45) NOT NULL,
  `date` date NOT NULL,
  `sorties` int(11) NOT NULL DEFAULT '0',
  `kills` int(11) NOT NULL DEFAULT '0',
  `deaths` int(11) NOT NULL DEFAULT '0',
  `slingloads` int(11) NOT NULL DEFAULT '0',
  `transport` int(11) NOT NULL DEFAULT '0',
  `resupplies` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `server`
--

DROP TABLE IF EXISTS `server`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `server` (
  `server_id` int(11) NOT NULL AUTO_INCREMENT,
  `map_id` int(11) DEFAULT NULL,
  `name` varchar(128) NOT NULL,
  `description` varchar(2000) NOT NULL COMMENT 'server description displayed on website',
  `ip_address` varchar(40) NOT NULL,
  `simple_radio_enabled` bit(1) NOT NULL DEFAULT b'0',
  `simple_radio_ip_address` varchar(40) NOT NULL,
  `restart_time` int(11) DEFAULT NULL,
  `status` varchar(10) DEFAULT NULL,
  `last_heartbeat` datetime DEFAULT NULL,
  PRIMARY KEY (`server_id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `session`
--

DROP TABLE IF EXISTS `session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `session` (
  `session_id` bigint(32) NOT NULL AUTO_INCREMENT,
  `server_id` int(11) NOT NULL,
  `start` datetime NOT NULL,
  `end` datetime DEFAULT NULL,
  `real_time_start` bigint(32) DEFAULT NULL,
  `real_time_end` bigint(32) DEFAULT NULL,
  `game_time_start` bigint(32) DEFAULT NULL,
  `game_time_end` bigint(32) DEFAULT NULL,
  `last_heartbeat` datetime DEFAULT NULL,
  PRIMARY KEY (`session_id`),
  KEY `server_id_idx` (`server_id`),
  CONSTRAINT `Session_ServerID` FOREIGN KEY (`server_id`) REFERENCES `server` (`server_id`) ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=666 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sproc_log`
--

DROP TABLE IF EXISTS `sproc_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sproc_log` (
  `sproc` varchar(128) DEFAULT NULL,
  `text` varchar(5000) DEFAULT NULL,
  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `target`
--

DROP TABLE IF EXISTS `target`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `target` (
  `target_id` int(11) NOT NULL AUTO_INCREMENT,
  `model` varchar(60) NOT NULL,
  `type` varchar(25) DEFAULT NULL COMMENT 'NOTE - type is nullable as not all DCS objects have a type ''ie buildings do not have a type to them, they are simply counted as STRUCTURES)',
  `category` varchar(15) NOT NULL,
  PRIMARY KEY (`target_id`)
) ENGINE=InnoDB AUTO_INCREMENT=89 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weapon`
--

DROP TABLE IF EXISTS `weapon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weapon` (
  `weapon_id` int(11) NOT NULL,
  `name` varchar(60) NOT NULL,
  `category` varchar(20) NOT NULL,
  PRIMARY KEY (`weapon_id`),
  UNIQUE KEY `unique_weapon` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'ki'
--
/*!50106 SET @save_time_zone= @@TIME_ZONE */ ;
/*!50106 DROP EVENT IF EXISTS `e_DeleteInactiveMissions` */;
DELIMITER ;;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;;
/*!50003 SET character_set_client  = utf8 */ ;;
/*!50003 SET character_set_results = utf8 */ ;;
/*!50003 SET collation_connection  = utf8_general_ci */ ;;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;;
/*!50003 SET @saved_time_zone      = @@time_zone */ ;;
/*!50003 SET time_zone             = 'SYSTEM' */ ;;
/*!50106 CREATE*/ /*!50117 DEFINER=`root`@`localhost`*/ /*!50106 EVENT `e_DeleteInactiveMissions` ON SCHEDULE EVERY 5 MINUTE STARTS '2017-12-26 21:40:45' ON COMPLETION NOT PRESERVE ENABLE COMMENT 'Deletes inactive missions' DO DELETE FROM side_mission
		WHERE status != "Active" AND fnc_GetMissionInactiveTimeInSeconds(side_mission_id) > 300 */ ;;
/*!50003 SET time_zone             = @saved_time_zone */ ;;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;;
/*!50003 SET character_set_client  = @saved_cs_client */ ;;
/*!50003 SET character_set_results = @saved_cs_results */ ;;
/*!50003 SET collation_connection  = @saved_col_connection */ ;;
/*!50106 DROP EVENT IF EXISTS `e_PlayerGainLife` */;;
DELIMITER ;;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;;
/*!50003 SET character_set_client  = utf8 */ ;;
/*!50003 SET character_set_results = utf8 */ ;;
/*!50003 SET collation_connection  = utf8_general_ci */ ;;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;;
/*!50003 SET @saved_time_zone      = @@time_zone */ ;;
/*!50003 SET time_zone             = 'SYSTEM' */ ;;
/*!50106 CREATE*/ /*!50117 DEFINER=`root`@`localhost`*/ /*!50106 EVENT `e_PlayerGainLife` ON SCHEDULE EVERY 1 HOUR STARTS '2017-12-15 14:19:05' ON COMPLETION NOT PRESERVE ENABLE COMMENT 'Restores 1 life to each player offline every hour' DO UPDATE player p
		LEFT JOIN online_players op
		ON op.ucid = p.ucid
			SET lives = lives + 1
		WHERE lives < 5 AND op.ucid IS NULL */ ;;
/*!50003 SET time_zone             = @saved_time_zone */ ;;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;;
/*!50003 SET character_set_client  = @saved_cs_client */ ;;
/*!50003 SET character_set_results = @saved_cs_results */ ;;
/*!50003 SET collation_connection  = @saved_col_connection */ ;;
/*!50106 DROP EVENT IF EXISTS `e_ServerStatusCheck` */;;
DELIMITER ;;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;;
/*!50003 SET character_set_client  = utf8 */ ;;
/*!50003 SET character_set_results = utf8 */ ;;
/*!50003 SET collation_connection  = utf8_general_ci */ ;;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;;
/*!50003 SET @saved_time_zone      = @@time_zone */ ;;
/*!50003 SET time_zone             = 'SYSTEM' */ ;;
/*!50106 CREATE*/ /*!50117 DEFINER=`root`@`localhost`*/ /*!50106 EVENT `e_ServerStatusCheck` ON SCHEDULE EVERY 5 MINUTE STARTS '2017-12-15 15:49:29' ON COMPLETION NOT PRESERVE ENABLE COMMENT 'Checks the status of servers' DO UPDATE server
		SET status = "Offline"
        WHERE fnc_GetLastHeartbeatInSeconds(server_id) > 300 AND status <> "Offline" */ ;;
/*!50003 SET time_zone             = @saved_time_zone */ ;;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;;
/*!50003 SET character_set_client  = @saved_cs_client */ ;;
/*!50003 SET character_set_results = @saved_cs_results */ ;;
/*!50003 SET collation_connection  = @saved_col_connection */ ;;
DELIMITER ;
/*!50106 SET TIME_ZONE= @save_time_zone */ ;

--
-- Dumping routines for database 'ki'
--
/*!50003 DROP FUNCTION IF EXISTS `fnc_GetLastHeartbeatInSeconds` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fnc_GetLastHeartbeatInSeconds`( ServerID INT) RETURNS int(11)
BEGIN
	SET @LastHeartbeat = 0;
	SELECT TIME_TO_SEC( TIMEDIFF( NOW(), COALESCE(last_heartbeat, FROM_UNIXTIME(0)) )) INTO @LastHeartbeat
	FROM server WHERE server_id = ServerID;
    
    RETURN @LastHeartbeat;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fnc_GetMissionInactiveTimeInSeconds` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fnc_GetMissionInactiveTimeInSeconds`( SideMissionID INT) RETURNS int(11)
BEGIN
	SET @TimeDiff = 0;
	SELECT TIME_TO_SEC( TIMEDIFF( NOW(), COALESCE(time_inactive, FROM_UNIXTIME(0)) )) INTO @TimeDiff
	FROM side_mission WHERE side_mission_id = SideMissionID;
    
    RETURN @TimeDiff;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fnc_GetRoleImage` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fnc_GetRoleImage`(role VARCHAR(45)) RETURNS varchar(132) CHARSET utf8
BEGIN
	DECLARE RoleImage VARCHAR(132);
    SELECT COALESCE(ri.image, "Images/role/role-none-30x30.png") INTO RoleImage 
    FROM role_image ri WHERE ri.role = role;
    
	RETURN RoleImage;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fnc_GetSessionLastHeartbeatInSeconds` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fnc_GetSessionLastHeartbeatInSeconds`( ServerID INT, SessionID INT) RETURNS int(11)
BEGIN
	SET @LastHeartbeat = 0;
	SELECT TIME_TO_SEC( TIMEDIFF( NOW(), COALESCE(last_heartbeat, FROM_UNIXTIME(0)) )) INTO @LastHeartbeat
	FROM session WHERE server_id = ServerID AND session_id = SessionID;
    
    RETURN @LastHeartbeat;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fnc_HoursToSeconds` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fnc_HoursToSeconds`(hours INT) RETURNS int(11)
BEGIN
	RETURN hours * POW(60, 2);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fnc_SESSION_LENGTH` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fnc_SESSION_LENGTH`() RETURNS int(11)
BEGIN
	RETURN fnc_HoursToSeconds(4);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AddConnectionEvent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddConnectionEvent`(
		ServerID INT,
        SessionID INT,
        Type VARCHAR(20),
        Name VARCHAR(128),
        UCID VARCHAR(128),
        ID INT,
        IP VARCHAR(25),
        GameTime BIGINT(32),
        RealTime BIGINT(32)
    )
BEGIN

	IF Type = "CONNECTED" THEN
		INSERT INTO online_players (server_id, ucid, name, role, side, ping)
		VALUES (ServerID, UCID, Name, "", 0, 0);
    ELSE
		DELETE FROM online_players WHERE online_players.server_id = ServerID AND online_players.ucid = UCID;
    END IF;
	INSERT INTO raw_Connection_log (server_id, session_id, type, player_ucid, player_name, player_id, ip_address, game_time, real_time)
    VALUES (ServerID, SessionID, Type, UCID, Name, ID, IP, GameTime, RealTime);
    SELECT LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AddGameEvent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddGameEvent`(
		IN ServerID INT, 
		IN SessionID BIGINT(32), 
        IN SortieID BIGINT(32), 
        IN UCID VARCHAR(128), 
        IN Event VARCHAR(45),
        IN PlayerName VARCHAR(128),
        IN PlayerSide INT,
        IN ModelTime BIGINT(32),
        IN GameTime BIGINT(32),
        IN Role VARCHAR(25),
        IN Location VARCHAR(60),
        IN Weapon VARCHAR(60),
        IN WeaponCategory VARCHAR(20),
        IN TargetName VARCHAR(60),
        IN TargetModel VARCHAR(60),
        IN TargetType VARCHAR(25),
        IN TargetCategory VARCHAR(15),
        IN TargetSide INT,
        IN TargetIsPlayer BIT(1),
        IN TargetPlayerUCID VARCHAR(128),
        IN TargetPlayerName VARCHAR(128),
        IN TransportUnloadedCount INT,
        IN Cargo VARCHAR(128)
	)
BEGIN
	INSERT INTO raw_gameevents_log (server_id, session_id, sortie_id, ucid, event, player_name, player_side, model_time, game_time, 
									role, location, weapon, weapon_category, target_name, target_model, target_type,
									target_category, target_side, target_is_player, target_player_ucid, target_player_name,
                                    transport_unloaded_count, cargo)
    VALUES(ServerID, SessionID, SortieID, UCID, Event, PlayerName, PlayerSide, ModelTime, GameTime, 
		   Role, Location, Weapon, WeaponCategory, TargetName, TargetModel, TargetType, 
           TargetCategory, TargetSide, TargetIsPlayer, TargetPlayerUCID, TargetPlayerName,
           TransportUnloadedCount, Cargo);
           
	SELECT LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BanPlayer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `BanPlayer`(
	UCID VARCHAR(128)
)
BEGIN
	UPDATE player SET banned = 1 WHERE player.ucid = UCID;
    SELECT UCID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `CreateSession` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `CreateSession`(
		ServerID INT,
        RealTimeStart BIGINT,
        GameTimeStart BIGINT,
        RefreshMissionData BOOL
    )
BEGIN
	IF RefreshMissionData THEN
		DELETE FROM capture_point WHERE server_id = ServerID;
        DELETE FROM depot WHERE server_id = ServerID;
	END IF;
	DELETE FROM online_players WHERE server_id = ServerID;
    UPDATE server SET status = "Online" WHERE server_id = ServerID;
	INSERT INTO session (server_id, start, real_time_start, game_time_start)
    VALUES (ServerID, NOW(), RealTimeStart, GameTimeStart);
    SELECT LAST_INSERT_ID() AS SessionID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `EndSession` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `EndSession`(
		ServerID INT,
        SessionID INT,
        RealTimeEnd BIGINT,
        GameTimeEnd BIGINT,
        ServerStatus VARCHAR(10)
    )
BEGIN
	DELETE FROM online_players WHERE server_id = ServerID;
    UPDATE session 
		SET end = NOW(), 
			real_time_end = RealTimeEnd,
            game_time_end = GameTimeEnd
		WHERE server_id = ServerID AND session_id = SessionID;
    UPDATE server SET status = ServerStatus WHERE server_id = ServerID;
    SELECT 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetOrAddPlayer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetOrAddPlayer`(
	UCID VARCHAR(128),
    Name VARCHAR(128)
)
BEGIN
	IF ((SELECT EXISTS (SELECT 1 FROM player WHERE player.ucid = UCID)) = 1) THEN
		SELECT player.ucid, player.name, lives, banned
        FROM player WHERE player.ucid = UCID;
	ELSE
		INSERT INTO rpt_overall_stats (ucid)
        VALUES(UCID);
        
		INSERT INTO player (player.ucid, player.name, lives, banned)
        VALUES (UCID, Name, 5, 0);
        SELECT player.ucid, player.name, lives, banned
        FROM player WHERE player.ucid = UCID;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetOrAddServer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetOrAddServer`(
		IN ServerName VARCHAR(128),
        IN Description VARCHAR(900),
        IN SimpleRadioEnabled BOOL,
        IN SimpleRadioIP VARCHAR(40),
        IN IP VARCHAR(30)
    )
BEGIN
	IF ((SELECT EXISTS (SELECT 1 FROM server WHERE server.ip_address = IP)) = 1) THEN
		UPDATE server 
			SET server.name = ServerName, 
				server.description = Description,
                server.simple_radio_enabled = SimpleRadioEnabled,
                server.simple_radio_ip_address = SimpleRadioIP
                WHERE ip_address = IP;
		SELECT server_id FROM server WHERE ip_address = IP;
    ELSE
		-- New Entry, Insert the new server into the database
        INSERT INTO server (name, description, ip_address, simple_radio_enabled, simple_radio_ip_address) 
        VALUES (ServerName, Description, IP, SimpleRadioEnabled, SimpleRadioIP);
        SELECT LAST_INSERT_ID();
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `IsPlayerBanned` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `IsPlayerBanned`(
	UCID VARCHAR(128)
)
BEGIN
    SELECT banned, player.ucid AS UCID FROM player WHERE player.ucid = UCID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `log` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `log`(sproc VARCHAR(128), text VARCHAR(5000))
BEGIN
	INSERT INTO sproc_log (sproc_log.sproc, sproc_log.text)
    VALUES (sproc, text);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetLast5SessionsBarGraph` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetLast5SessionsBarGraph`(IN UCID VARCHAR(128))
BEGIN   
	SELECT 
	s.session_id AS SessionID,
	s.event AS Event,
	COUNT(s.event) AS EventCount
	FROM rpt_player_session_series s
	RIGHT JOIN
		( 	
			SELECT DISTINCT session_id
			FROM rpt_player_session_series sss
			WHERE sss.ucid = UCID
			ORDER BY session_id DESC
			LIMIT 5
		 ) ss
		ON s.session_id = ss.session_id
	WHERE (Event = "KILL" OR Event = "TAKEOFF" OR Event = "LAND") AND s.ucid = UCID
	GROUP BY SessionID, Event
    ORDER BY SessionID ASC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetLastSessionSeries` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetLastSessionSeries`(IN UCID VARCHAR(128))
BEGIN   
    -- DECLARE SessionID INT;
    -- SET SessionID = 66;
	SELECT 
		s.event AS Event,
		s.game_time - ss.game_time_start AS Time
	FROM rpt_player_session_series s
    INNER JOIN session ss
		ON s.session_id = ss.session_id
	WHERE s.ucid = UCID AND s.session_id = 
		 ( 	
			SELECT MAX(session_id) 
			FROM rpt_player_session_series sss
            WHERE sss.ucid = UCID
		 );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetOnlineActivity` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetOnlineActivity`(IN UCID VARCHAR(128))
BEGIN
	SELECT 
		date AS Date,
        total_game_time AS TotalTime
    FROM ki.rpt_player_online_activity rpt
    WHERE rpt.ucid = UCID
	ORDER BY date ASC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetPlayerAirframeStatsBasic` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetPlayerAirframeStatsBasic`(IN UCID VARCHAR(128))
BEGIN
	SELECT 
        a.airframe AS Airframe,
        a.flight_time AS Time,
        a.takeoffs AS Sorties,
        'N/A' AS Top
	FROM rpt_airframe_stats a
    WHERE a.ucid = UCID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetPlayerBestSortieStats` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetPlayerBestSortieStats`(IN UCID VARCHAR(128))
BEGIN
	SELECT 
	(
		SELECT MAX(kills) FROM rpt_airframe_sortie WHERE ucid = UCID
	) AS MostKills,
	(
		SELECT MAX(sortie_time) FROM rpt_airframe_sortie WHERE ucid = UCID
	) AS LongestSortie,
	(
		SELECT MAX(hits_received) FROM rpt_airframe_sortie WHERE ucid = UCID
	) AS MostHitsReceived;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetPlayerOverallStats` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetPlayerOverallStats`(IN UCID VARCHAR(128))
BEGIN
	SELECT 
        p.name AS PlayerName,
        p.lives AS PlayerLives,
        p.banned AS PlayerBanned,
		game_time AS TotalGameTime,
        takeoffs AS TakeOffs,
        landings AS Landings,
        slingload_hooks AS SlingLoadHooks,
        slingload_unhooks AS SlingLoadUnhooks,
        kills AS Kills,
        deaths AS Deaths,
        ejects AS Ejects,
        transport_mounts AS TransportMounts,
        transport_dismounts AS TransportDismounts,
        depot_resupplies AS DepotResupplies,
        cargo_unpacked AS CargoUnpacked,
        landings / takeoffs AS SortieSuccessRatio,
        slingload_unhooks / slingload_hooks AS SlingLoadSuccessRatio,
        kills / CASE WHEN (deaths + ejects) = 0 THEN 1 ELSE (deaths + ejects) END AS KillDeathEjectRatio,
        transport_dismounts / transport_mounts AS TransportSuccessRatio,
        (
			SELECT COALESCE(SUM(rpt.kills), 0)
            FROM rpt_airframe_kd rpt
            WHERE rpt.ucid = UCID AND rpt.name = 'GROUND'
        ) AS GroundKills,
        (
			SELECT COALESCE(SUM(rpt.kills), 0)
            FROM rpt_airframe_kd rpt
            WHERE rpt.ucid = UCID AND rpt.name = 'SHIP'
        ) AS ShipKills,
        (
			SELECT COALESCE(SUM(rpt.kills), 0)
            FROM rpt_airframe_kd rpt
            WHERE rpt.ucid = UCID AND rpt.name = 'HELICOPTER'
        ) AS HelicopterKills,
        (
			SELECT COALESCE(SUM(rpt.kills), 0)
            FROM rpt_airframe_kd rpt
            WHERE rpt.ucid = UCID AND rpt.name = 'AIR'
        ) AS AirKills
	FROM rpt_overall_stats r
    INNER JOIN player p
    ON r.ucid = p.ucid
    WHERE r.ucid = UCID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetScoreOverTime` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetScoreOverTime`(IN UCID VARCHAR(128))
BEGIN
	SELECT 
        r.date AS Date,
        r.kills AS Kills,
        r.slingloads AS SlingLoads,
        r.transport AS Transport,
        r.resupplies AS Resupplies
	FROM rpt_sorties_over_time r
    WHERE r.ucid = UCID AND r.airframe = 'TOTAL'
    ORDER BY r.date;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetSortiesOverTime` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetSortiesOverTime`(IN UCID VARCHAR(128))
BEGIN
	SELECT 
		r.airframe AS Airframe,
        r.date AS Date,
        r.sorties AS Sorties,
        r.kills AS Kills,
        r.deaths AS Deaths
	FROM rpt_sorties_over_time r
    WHERE r.ucid = UCID
    ORDER BY 
		CASE 
			WHEN r.airframe="TOTAL" THEN 0
			ELSE 1 
		END,
        r.airframe, r.date;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `rptsp_GetTopAirframeSeries` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `rptsp_GetTopAirframeSeries`(IN UCID VARCHAR(128), IN RowLimit INT)
BEGIN
	SELECT 
		a.airframe AS Airframe,
        a.flight_time AS TotalTime
	FROM rpt_airframe_stats a
    WHERE a.ucid = UCID
    LIMIT RowLimit;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SendHeartbeat` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SendHeartbeat`(
		IN ServerID INT,
        IN SessionID INT,
        IN RestartTime INT
    )
BEGIN
	UPDATE server 
		SET last_heartbeat = NOW(),
        status = "Online",
		restart_time = RestartTime
	WHERE server_id = ServerID;
    UPDATE session
		SET last_heartbeat = NOW()
	WHERE server_id = ServerID and session_id = SessionID;
	SELECT 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `UnbanPlayer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `UnbanPlayer`(
	UCID VARCHAR(128)
)
BEGIN
	UPDATE player SET banned = 0 WHERE player.ucid = UCID;
    SELECT UCID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `UpdatePlayer` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdatePlayer`(
	ServerID INT,
	UCID VARCHAR(128),
    Name VARCHAR(128),
    Role VARCHAR(45),
    Lives INT,
    Side INT,
    Ping INT
)
BEGIN
	UPDATE player
    SET player.lives = Lives, player.name = Name
    WHERE player.ucid = UCID;
    
    UPDATE online_players
    SET online_players.role = Role, online_players.side = Side, online_players.ping = Ping
    WHERE online_players.server_id = ServerID AND online_players.ucid = UCID;
    
    SELECT UCID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `websp_GetCustomMenuItems` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `websp_GetCustomMenuItems`(ServerID INT)
BEGIN
	SELECT c.menu_name AS MenuName,
		   c.icon_class AS IconClass,
           c.html_content AS HtmlContent
	FROM custom_menu_item c
    WHERE c.server_id = ServerID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `websp_GetGame` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `websp_GetGame`(ServerID INT)
BEGIN
	SELECT s.server_id as ServerID, 
		   s.name as ServerName, 
           s.description as ServerDescription,
           s.ip_address as IPAddress,  
           s.simple_radio_enabled as SimpleRadioEnabled,
           s.simple_radio_ip_address as SimpleRadioIPAddress,
           COUNT(op.ucid) as OnlinePlayerCount,
           s.restart_time as RestartTime,
           s.status as Status,
           m.name as Map
	FROM server s
    LEFT JOIN online_players op
		ON s.server_id = op.server_id
	LEFT JOIN map m
		ON s.map_id = m.map_id
    WHERE s.server_id = ServerID
    GROUP BY s.server_id, s.name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `websp_GetServerInfo` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `websp_GetServerInfo`(ServerID INT)
BEGIN
	SELECT s.server_id as ServerID, 
           s.restart_time as RestartTime,
           s.status
	FROM server s
    WHERE s.server_id = ServerID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `websp_GetServersList` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `websp_GetServersList`()
BEGIN
	SELECT s.server_id as ServerID, 
		   s.name as ServerName, 
           s.ip_address as IPAddress,  
           COUNT(op.ucid) as OnlinePlayers,
           s.restart_time as RestartTime,
           s.status
	FROM server s
    LEFT JOIN online_players op
		ON s.server_id = op.server_id
	GROUP BY s.server_id, s.name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `websp_SearchPlayers` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `websp_SearchPlayers`(IN Criteria VARCHAR(128))
BEGIN
	SELECT player.ucid AS UCID,
		   player.name AS Name,
           player.banned AS Banned,
           COALESCE(stats.game_time, 0) AS GameTime,
           COALESCE(stats.takeoffs, 0) AS Sorties,
           COALESCE(stats.kills, 0) AS Kills
    FROM player 
    LEFT JOIN rpt_overall_stats stats
    ON stats.ucid = player.ucid
    WHERE LOWER(player.name) LIKE CONCAT("%", LOWER(Criteria), "%");
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `websp_SearchServers` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `websp_SearchServers`(IN Criteria VARCHAR(128))
BEGIN
	SELECT s.server_id as ServerID, 
		   s.name as ServerName, 
           s.ip_address as IPAddress,  
           COUNT(op.ucid) as OnlinePlayers,
           s.restart_time as RestartTime,
           s.status
	FROM server s
    LEFT JOIN online_players op
		ON s.server_id = op.server_id
	WHERE LOWER(s.name) LIKE CONCAT("%", LOWER(Criteria), "%")
	GROUP BY s.server_id, s.name;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `websp_SearchTotals` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `websp_SearchTotals`(IN Criteria VARCHAR(128))
BEGIN
	SELECT 
		(
			SELECT COUNT(*)
			FROM player 
			WHERE LOWER(player.name) LIKE CONCAT("%", LOWER(Criteria), "%")
		) AS PlayerResults,
		(
			SELECT COUNT(*)
			FROM server 
			WHERE LOWER(server.name) LIKE CONCAT("%", LOWER(Criteria), "%")
		) AS ServerResults
	FROM dual;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-07-06  5:22:23
