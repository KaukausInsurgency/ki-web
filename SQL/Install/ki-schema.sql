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
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;
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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-06-29 14:28:34
