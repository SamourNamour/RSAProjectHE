CREATE DATABASE  IF NOT EXISTS `sa_mebs` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `sa_mebs`;
-- MySQL dump 10.13  Distrib 5.6.13, for Win32 (x86)
--
-- Host: localhost    Database: sa_mebs
-- ------------------------------------------------------
-- Server version	5.5.9

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
-- Table structure for table `mebs_category`
--

DROP TABLE IF EXISTS `mebs_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_category` (
  `IdCategory` int(10) NOT NULL AUTO_INCREMENT,
  `Value` varchar(300) DEFAULT NULL,
  `Visibility` varchar(30) DEFAULT NULL,
  `ParentID` int(10) DEFAULT '0',
  `Orden` int(10) DEFAULT NULL,
  `Default` int(11) DEFAULT '-1',
  `MediasetLCN` int(11) DEFAULT '-1',
  `StandardLCN` int(11) DEFAULT '-1',
  `IsUnclass` bit(1) DEFAULT b'0',
  `IsMixed` bit(1) DEFAULT b'0',
  `IsPublished` bit(1) DEFAULT b'1',
  `DateCreation` datetime NOT NULL,
  PRIMARY KEY (`IdCategory`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_category`
--

LOCK TABLES `mebs_category` WRITE;
/*!40000 ALTER TABLE `mebs_category` DISABLE KEYS */;
INSERT INTO `mebs_category` VALUES (1,'1','false',0,0,-1,-1,-1,'\0','\0','','2012-09-04 10:00:00'),(2,'2','false',0,1,-1,-1,-1,'\0','\0','','2012-09-04 10:00:00'),(3,'13203','false',0,2,6,-1,-1,'\0','\0','','2012-09-04 10:00:00'),(4,'13204','true',0,5,4,-1,-1,'\0','','','2012-09-04 10:00:00'),(5,'13205','true',0,3,0,-1,-1,'\0','\0','','2012-09-04 10:00:00'),(6,'13206','true',0,4,-1,-1,-1,'\0','\0','','2012-09-04 10:00:00'),(7,'13207','true',0,6,1,-1,-1,'\0','\0','','2012-09-04 10:00:00'),(8,'49207','true',0,7,3,-1,-1,'\0','\0','','2013-04-30 17:01:51'),(9,'4400','always',0,8,5,-1,-1,'\0','\0','','2013-04-30 17:14:34');
/*!40000 ALTER TABLE `mebs_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_category_language_mapping`
--

DROP TABLE IF EXISTS `mebs_category_language_mapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_category_language_mapping` (
  `IdCategoryDetails` int(10) NOT NULL AUTO_INCREMENT,
  `IdCategory` int(10) DEFAULT NULL,
  `IdLanguage` int(10) DEFAULT NULL,
  `Title` varchar(300) DEFAULT NULL,
  `VirtualChannelDisignation` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`IdCategoryDetails`),
  KEY `FK_IdCategory1` (`IdCategory`),
  KEY `FK_IdLanguage` (`IdLanguage`),
  CONSTRAINT `FK_IdCategory1` FOREIGN KEY (`IdCategory`) REFERENCES `mebs_category` (`IdCategory`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_IdLanguage` FOREIGN KEY (`IdLanguage`) REFERENCES `mebs_language` (`IdLanguage`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_category_language_mapping`
--

LOCK TABLES `mebs_category_language_mapping` WRITE;
/*!40000 ALTER TABLE `mebs_category_language_mapping` DISABLE KEYS */;
INSERT INTO `mebs_category_language_mapping` VALUES (1,1,1,'All',' '),(2,2,1,'Unclassified',' '),(3,3,1,'Serie',' '),(4,4,1,'Sport','Sport'),(5,5,1,'Trailer',' '),(6,6,1,'Animation',' '),(7,7,1,'Romantique',' '),(8,8,1,'News','Category Name'),(9,9,1,'Movie','Category Name 2');
/*!40000 ALTER TABLE `mebs_category_language_mapping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_channel`
--

DROP TABLE IF EXISTS `mebs_channel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_channel` (
  `IdChannel` int(10) NOT NULL AUTO_INCREMENT,
  `Bus` varchar(50) DEFAULT NULL,
  `LongName` varchar(150) DEFAULT NULL,
  `ShortName` varchar(50) DEFAULT NULL,
  `ChannelType` tinyint(4) DEFAULT NULL,
  `ChannelKey` varchar(100) DEFAULT NULL COMMENT 'TSID_ONID_SID',
  `LogoFileName` varchar(50) DEFAULT NULL,
  `Logo` blob,
  `DateCreation` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `XmlFileName` varchar(255) DEFAULT NULL,
  `Enabled` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`IdChannel`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_channel`
--

LOCK TABLES `mebs_channel` WRITE;
/*!40000 ALTER TABLE `mebs_channel` DISABLE KEYS */;
INSERT INTO `mebs_channel` VALUES (30,'CHNL_VOD','PVOD','PVOD',1,'CHNL_VOD',NULL,NULL,'2013-04-29 16:13:30',NULL,1);
/*!40000 ALTER TABLE `mebs_channel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_channeltuning`
--

DROP TABLE IF EXISTS `mebs_channeltuning`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_channeltuning` (
  `IdChannelTuning` int(10) NOT NULL AUTO_INCREMENT,
  `IdChannel` int(10) NOT NULL,
  `ServiceID` int(10) unsigned NOT NULL,
  `TransportStreamID` int(10) unsigned NOT NULL,
  `OriginalNetworkID` int(10) unsigned NOT NULL,
  PRIMARY KEY (`IdChannelTuning`),
  KEY `FK_IdChannelAss` (`IdChannel`),
  CONSTRAINT `FK_IdChannelAss` FOREIGN KEY (`IdChannel`) REFERENCES `mebs_channel` (`IdChannel`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_channeltuning`
--

LOCK TABLES `mebs_channeltuning` WRITE;
/*!40000 ALTER TABLE `mebs_channeltuning` DISABLE KEYS */;
INSERT INTO `mebs_channeltuning` VALUES (41,30,1250,1250,1250);
/*!40000 ALTER TABLE `mebs_channeltuning` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_encapsulator`
--

DROP TABLE IF EXISTS `mebs_encapsulator`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_encapsulator` (
  `IdEncapsulator` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(250) DEFAULT NULL,
  `Type` varchar(250) DEFAULT NULL,
  `Status` int(10) unsigned DEFAULT NULL,
  `IsPublished` bit(1) DEFAULT NULL,
  `IpAddress` varchar(250) DEFAULT NULL,
  `MultiInstancesNum` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`IdEncapsulator`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_encapsulator`
--

LOCK TABLES `mebs_encapsulator` WRITE;
/*!40000 ALTER TABLE `mebs_encapsulator` DISABLE KEYS */;
INSERT INTO `mebs_encapsulator` VALUES (1,'Main Metadata','Metadata',1,'','127.0.0.1',0),(2,'Main Asset','Asset',1,'','127.0.0.1',0);
/*!40000 ALTER TABLE `mebs_encapsulator` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_ingesta`
--

DROP TABLE IF EXISTS `mebs_ingesta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_ingesta` (
  `IdIngesta` int(10) NOT NULL AUTO_INCREMENT,
  `EventId` varchar(255) DEFAULT NULL,
  `Code_Package` varchar(255) DEFAULT NULL,
  `Type` tinyint(4) DEFAULT NULL COMMENT 'BesTVInfo 3.0  =  Type :Positive integer number in the range 0-65535 that specifies the type of content. AR=0 ; DC=1',
  `IdChannel` int(10) NOT NULL,
  `Date_Creation` datetime DEFAULT NULL,
  `Estimated_Start` datetime DEFAULT NULL,
  `Estimated_Stop` datetime DEFAULT NULL,
  `Expiration_time` datetime DEFAULT NULL COMMENT 'BesTVInfo 3.0  = ExpiresAfter : Positive integer number specifying the amount of time (in minutes) after which the content will expire (starting the countdown at the time of contentâs creation).',
  `Immortality_time` datetime DEFAULT NULL COMMENT 'BesTVInfo 3.0  = ImmortalDuring :Positive integer number specifying the amount of time (in minutes) during which the content will be immortal (starting the countdown at the time of contentâs creation). This value cannot be greater than âExpiresAfterâ.',
  `Validity_time` int(10) NOT NULL,
  `AvailableAfter_time` int(10) NOT NULL,
  `Title` varchar(300) DEFAULT NULL COMMENT 'BesTVInfo 3.0  =  Name : Title or name of the content.',
  `ParentalRating` varchar(45) DEFAULT 'OFF',
  `PosterFileExtension` varchar(100) DEFAULT NULL,
  `Last_Update` datetime DEFAULT NULL,
  `IsExpired` bit(1) NOT NULL,
  `Poster` longblob,
  `Duration` varchar(50) DEFAULT '0',
  `IsPublished` bit(1) DEFAULT b'0',
  `XmlFileName` varchar(255) DEFAULT NULL,
  `MinLifeAfterFirstAccess` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = MinLifeAfterFirstAccess : Positive integer number specifying the minimum life of the content after it is watched for the first time (in minutes). If zero, this field is not taken into account.',
  `LifeAfterFirstAccess` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = LifeAfterFirstAccess : Positive integer number specifying the life of the content after it is watched for the first time (in minutes). If zero, this field is not taken into account.',
  `MinLifeAfterActivation` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = MinLifeAfterActivation : Positive integer number specifying the minimum life of the content after it is activated (in minutes). If zero, this field is not taken into account.',
  `LifeAfterActivation` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = LifeAfterActivation : Positive integer number specifying the life of the content after it is activated (in minutes). If zero, this field is not taken into account.',
  `DisableAccess` bit(1) DEFAULT b'0' COMMENT 'BesTVInfo 3.0  = DisableAccess : If âtrueâ the content wonât be accessible by default. This setting overrides the rest of content access parameters if it is âtrueâ.',
  `ActiveSince` varchar(50) DEFAULT '' COMMENT 'BesTVInfo 3.0  = ActiveSince : String representing a date in the format âdd/mm/yyyy hh:mm:ssâ that is used as the date when the content will start being active. If empty, the creation date of the content is used instead.',
  `ActiveDuring` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = ActiveDuring : Positive integer number specifying the amount of time (in minutes) during which the content will be active (starting the countdown at the time of contentâs creation or starting from âActiveSinceâ if it is specified).',
  `ActiveTimeAfterFirstAccess` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = ActiveTimeAfterFirstAccess : Positive integer number specifying the time that the content will be active after it has been watched for the first time (after every activation process). If zero, this field is not taken into account.',
  `MinActiveTimeAfterFirstAccess` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = MinActiveTimeAfterFirstAccess : Positive integer number specifying the minimum time that the content will be active after it has been watched for the first time (after every activation process). Used if greater than zero and ActiveTimeAfterFirstAccess is zero.',
  `DrmProtected` bit(1) DEFAULT b'0' COMMENT 'BesTVInfo 3.0  = DrmProtected : if âtrueâ, indicates that the content is protected by a DRM system.',
  `CopyControl` varchar(45) DEFAULT 'true' COMMENT 'BesTVInfo 3.0  = CopyControl : indicates the copy control restriction to be applied for this content. Its possible values are: \\"allow\\", \\"copy once\\", \\"copy no more\\", \\"copy never\\", \\"copy never zero retention\\".',
  `MaxAccesses` int(10) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = MaxAccesses : Integer number specifying the number of times the user is allowed to access the content. After it has been accessed MaxAccesses times, the content will be inactive. If zero, content is inactive since its creation. If -1, the user is allowed to access this content an infinite number of times.',
  `Hidden` bit(1) DEFAULT b'0' COMMENT 'BesTVInfo 3.0  = Hidden:  âtrueâ or âfalseâ.',
  `PublishAfter` int(11) DEFAULT '0' COMMENT 'BesTVInfo 3.0  = PublishAfter : Positive integer number specifying the amount of time (in minutes) after which the content will be published (starting the countdown at the time of contents creation).',
  `SelfCommercial` int(10) DEFAULT '-1',
  `MediaFileNameAfterRedundancy` varchar(150) NOT NULL,
  `MediaFileSizeAfterRedundancy` bigint(20) unsigned NOT NULL DEFAULT '0',
  `OriginalFileName` varchar(255) DEFAULT '',
  `PreservationPriority` int(10) DEFAULT '0',
  PRIMARY KEY (`IdIngesta`),
  KEY `FK_IdChannel` (`IdChannel`),
  CONSTRAINT `FK_IdChannel` FOREIGN KEY (`IdChannel`) REFERENCES `mebs_channel` (`IdChannel`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9853 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_ingesta`
--

LOCK TABLES `mebs_ingesta` WRITE;
/*!40000 ALTER TABLE `mebs_ingesta` DISABLE KEYS */;
/*!40000 ALTER TABLE `mebs_ingesta` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_INSERT_MEBS_INGESTA` BEFORE INSERT ON `mebs_ingesta` FOR EACH ROW BEGIN

DECLARE _MediaFileNameAfterRedundancy VARCHAR(255) DEFAULT '';

DECLARE _FileSizeAfterRedundancy BIGINT(20) DEFAULT 0;



SELECT FileSizeAfterRedundancy , RedundancyFileName INTO _FileSizeAfterRedundancy , _MediaFileNameAfterRedundancy 

FROM mebs_mediafile where OriginalFileName = NEW.OriginalFileName LIMIT 1;



IF(_MediaFileNameAfterRedundancy != '') THEN

	 

    SET NEW.MediaFileNameAfterRedundancy = _MediaFileNameAfterRedundancy;

	SET NEW.MediaFileSizeAfterRedundancy = _FileSizeAfterRedundancy;



END IF;



END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `mebs_ingesta_advertisement_mapping`
--

DROP TABLE IF EXISTS `mebs_ingesta_advertisement_mapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_ingesta_advertisement_mapping` (
  `IdIngestaAdvertisement` int(10) NOT NULL AUTO_INCREMENT,
  `IdIngesta` int(10) NOT NULL,
  `IdSchedule` int(10) NOT NULL,
  `ActionName` varchar(255) DEFAULT NULL,
  `StartTimePoint` varchar(250) DEFAULT NULL,
  `StopTimePoint` varchar(250) DEFAULT NULL,
  `MaxFwdSpeed` int(10) DEFAULT NULL,
  `MaxRwdSpeed` int(10) DEFAULT NULL,
  `CanSkip` bit(1) DEFAULT NULL,
  `DateCreation` datetime DEFAULT NULL,
  PRIMARY KEY (`IdIngestaAdvertisement`),
  KEY `FK_Ingesta_Advertisement_idx` (`IdIngesta`),
  KEY `FK_Schedule_Advertisement_idx` (`IdSchedule`),
  CONSTRAINT `FK_Ingesta_Advertisement` FOREIGN KEY (`IdIngesta`) REFERENCES `mebs_ingesta` (`IdIngesta`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_Schedule_Advertisement` FOREIGN KEY (`IdSchedule`) REFERENCES `mebs_schedule` (`IdSchedule`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_ingesta_advertisement_mapping`
--

LOCK TABLES `mebs_ingesta_advertisement_mapping` WRITE;
/*!40000 ALTER TABLE `mebs_ingesta_advertisement_mapping` DISABLE KEYS */;
/*!40000 ALTER TABLE `mebs_ingesta_advertisement_mapping` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_INSERT_MEBS_INGESTA_ADVERTISEMENT_MAPPING`
AFTER INSERT ON `mebs_ingesta_advertisement_mapping`
FOR EACH ROW
BEGIN
DECLARE TOTAL_RW INTEGER ;

/* Set the The LastNotificationDate &  LastNotificationAction before insertion of new schedule.*/

SELECT COUNT(*) INTO TOTAL_RW FROM mebs_ingesta WHERE new.IdIngesta = IdIngesta AND SelfCommercial =0;
IF (TOTAL_RW > 0) THEN
	UPDATE mebs_ingesta SET SelfCommercial = 1 WHERE new.IdIngesta = IdIngesta;
END IF;


END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_DELETE_MEBS_INGESTA_ADVERTISEMENT_MAPPING`
AFTER DELETE ON `mebs_ingesta_advertisement_mapping`
FOR EACH ROW
BEGIN
DECLARE TOTAL_RW INTEGER ;

/* Set the The LastNotificationDate &  LastNotificationAction before insertion of new schedule.*/

SELECT COUNT(*) INTO TOTAL_RW FROM mebs_ingesta_advertisement_mapping WHERE OLD.IdIngesta = IdIngesta ;
IF (TOTAL_RW = 0) THEN
	UPDATE mebs_ingesta SET SelfCommercial = 0 WHERE OLD.IdIngesta = IdIngesta;
END IF;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `mebs_ingesta_category_mapping`
--

DROP TABLE IF EXISTS `mebs_ingesta_category_mapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_ingesta_category_mapping` (
  `IdIngestaCategory` int(10) NOT NULL AUTO_INCREMENT,
  `IdIngesta` int(10) NOT NULL,
  `IdCategory` int(10) NOT NULL,
  PRIMARY KEY (`IdIngestaCategory`),
  KEY `FK_IdIngestaAss` (`IdIngesta`),
  KEY `FK_IdCategoryAss` (`IdCategory`),
  CONSTRAINT `FK_IdCategoryAss` FOREIGN KEY (`IdCategory`) REFERENCES `mebs_category` (`IdCategory`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_IdIngestaAss` FOREIGN KEY (`IdIngesta`) REFERENCES `mebs_ingesta` (`IdIngesta`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=16597 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_ingesta_category_mapping`
--

LOCK TABLES `mebs_ingesta_category_mapping` WRITE;
/*!40000 ALTER TABLE `mebs_ingesta_category_mapping` DISABLE KEYS */;
/*!40000 ALTER TABLE `mebs_ingesta_category_mapping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_ingestadetails`
--

DROP TABLE IF EXISTS `mebs_ingestadetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_ingestadetails` (
  `IdIngestaDetails` int(10) NOT NULL AUTO_INCREMENT,
  `IdIngesta` int(10) NOT NULL,
  `DetailsName` varchar(100) DEFAULT NULL,
  `DetailsValue` text,
  PRIMARY KEY (`IdIngestaDetails`),
  KEY `FK_IdIngesta` (`IdIngesta`),
  CONSTRAINT `FK_Ingesta` FOREIGN KEY (`IdIngesta`) REFERENCES `mebs_ingesta` (`IdIngesta`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=87029 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_ingestadetails`
--

LOCK TABLES `mebs_ingestadetails` WRITE;
/*!40000 ALTER TABLE `mebs_ingestadetails` DISABLE KEYS */;
/*!40000 ALTER TABLE `mebs_ingestadetails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_language`
--

DROP TABLE IF EXISTS `mebs_language`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_language` (
  `IdLanguage` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(300) DEFAULT NULL,
  `LanguageCulture` varchar(50) DEFAULT NULL,
  `ISOCode` varchar(20) DEFAULT NULL,
  `IsPublished` bit(1) DEFAULT NULL,
  `DateCreation` datetime NOT NULL,
  PRIMARY KEY (`IdLanguage`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_language`
--

LOCK TABLES `mebs_language` WRITE;
/*!40000 ALTER TABLE `mebs_language` DISABLE KEYS */;
INSERT INTO `mebs_language` VALUES (1,'English','eng','eng','','2012-09-04 10:00:00');
/*!40000 ALTER TABLE `mebs_language` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_login`
--

DROP TABLE IF EXISTS `mebs_login`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_login` (
  `UserId` int(10) NOT NULL AUTO_INCREMENT,
  `UserGUID` varchar(255) DEFAULT NULL,
  `Login` varchar(255) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `PasswordQuestion` varchar(255) DEFAULT NULL,
  `PasswordAnswer` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT NULL,
  `LastActivityDate` datetime DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_login`
--

LOCK TABLES `mebs_login` WRITE;
/*!40000 ALTER TABLE `mebs_login` DISABLE KEYS */;
INSERT INTO `mebs_login` VALUES (13,'4d362390-7e74-4ba4-9a2b-9922c9600280','Administrator','MEBSAdmin','','','',1,'2014-04-23 12:45:11','2014-04-23 12:40:36'),(14,'dd553500-8ae8-466c-89cf-315278088e3a','user','123.123','City','Mohammedia','kgoubar@motivetelevision.co.uk',1,'2014-05-05 11:13:14','2014-05-05 12:08:46');
/*!40000 ALTER TABLE `mebs_login` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_mediafile`
--

DROP TABLE IF EXISTS `mebs_mediafile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_mediafile` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `GUID` varchar(45) NOT NULL,
  `OriginalFileName` varchar(150) NOT NULL DEFAULT '',
  `FileSize` bigint(20) unsigned NOT NULL,
  `FileSizeAfterRedundancy` bigint(20) unsigned NOT NULL,
  `CreationDateTime` datetime NOT NULL,
  `RedundancyStatus` int(10) unsigned NOT NULL,
  `RedundancyFileName` varchar(150) NOT NULL DEFAULT '',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_mediafile`
--

LOCK TABLES `mebs_mediafile` WRITE;
/*!40000 ALTER TABLE `mebs_mediafile` DISABLE KEYS */;
/*!40000 ALTER TABLE `mebs_mediafile` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_INSERT_MEBS_MEDIAFILE` AFTER INSERT ON `mebs_mediafile` FOR EACH ROW BEGIN

DECLARE _IdIngesta INT DEFAULT -1;



SELECT idingesta INTO _IdIngesta FROM mebs_ingesta where OriginalFileName = NEW.OriginalFileName LIMIT 1;



IF(_IdIngesta > 0) THEN

	 

	UPDATE mebs_ingesta SET MediaFileNameAfterRedundancy = NEW.RedundancyFileName , MediaFileSizeAfterRedundancy= NEW.FileSizeAfterRedundancy

	WHERE IdIngesta = _IdIngesta;



END IF;



END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `mebs_mixedcategory`
--

DROP TABLE IF EXISTS `mebs_mixedcategory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_mixedcategory` (
  `IdMixedCategory` int(10) NOT NULL AUTO_INCREMENT,
  `IdParentCategory` int(10) DEFAULT NULL,
  `IdChildCategory` int(10) DEFAULT NULL,
  `Orden` int(10) DEFAULT NULL,
  `IsDefault` bit(1) DEFAULT NULL,
  `DateCreation` datetime NOT NULL,
  PRIMARY KEY (`IdMixedCategory`),
  KEY `FK_IdChildCategoria` (`IdChildCategory`),
  KEY `FK_IdParentCategoria` (`IdParentCategory`),
  CONSTRAINT `FK_IdChildCategoria` FOREIGN KEY (`IdChildCategory`) REFERENCES `mebs_category` (`IdCategory`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `FK_IdParentCategoria` FOREIGN KEY (`IdParentCategory`) REFERENCES `mebs_category` (`IdCategory`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_mixedcategory`
--

LOCK TABLES `mebs_mixedcategory` WRITE;
/*!40000 ALTER TABLE `mebs_mixedcategory` DISABLE KEYS */;
INSERT INTO `mebs_mixedcategory` VALUES (15,4,3,0,'\0','2013-05-06 12:59:03'),(17,4,6,0,'\0','2013-05-06 12:59:12');
/*!40000 ALTER TABLE `mebs_mixedcategory` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_INSERT_MEBS_MIXECATEGORY` AFTER INSERT ON `mebs_mixedcategory` FOR EACH ROW BEGIN



DECLARE TOTAL_ROW INTEGER;

SELECT COUNT(*) INTO TOTAL_ROW FROM `mebs_mixedcategory` WHERE IdParentCategory = new.IdParentCategory;

IF (TOTAL_ROW >= 2) THEN



update mebs_category 

     SET IsMixed = true 

     WHERE IdCategory= new.IdParentCategory;  



END IF;



END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_DELETE_MEBS_MIXECATEGORY` AFTER DELETE ON `mebs_mixedcategory` FOR EACH ROW BEGIN



DECLARE TOTAL_ROW INTEGER;

SELECT COUNT(*) INTO TOTAL_ROW FROM `mebs_mixedcategory` WHERE IdParentCategory = OLD.IdParentCategory;

IF (TOTAL_ROW < 2) THEN



update mebs_category 

     SET IsMixed = false 

     WHERE IdCategory= OLD.IdParentCategory;  



END IF;



END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `mebs_roles`
--

DROP TABLE IF EXISTS `mebs_roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_roles` (
  `IdRole` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(250) DEFAULT NULL,
  PRIMARY KEY (`IdRole`)
) ENGINE=InnoDB AUTO_INCREMENT=153 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_roles`
--

LOCK TABLES `mebs_roles` WRITE;
/*!40000 ALTER TABLE `mebs_roles` DISABLE KEYS */;
INSERT INTO `mebs_roles` VALUES (151,'MEBSAdmin'),(152,'MEBSMAM');
/*!40000 ALTER TABLE `mebs_roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_schedule`
--

DROP TABLE IF EXISTS `mebs_schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_schedule` (
  `IdSchedule` int(10) NOT NULL AUTO_INCREMENT,
  `IdIngesta` int(10) NOT NULL,
  `EventId` varchar(255) DEFAULT NULL,
  `ContentID` int(10) DEFAULT NULL,
  `Date_Schedule` datetime DEFAULT NULL,
  `Estimated_Start` datetime DEFAULT NULL,
  `Estimated_Stop` datetime DEFAULT NULL,
  `Exact_Start` datetime DEFAULT NULL,
  `Exact_Stop` datetime DEFAULT NULL,
  `IsActive` bit(1) DEFAULT NULL,
  `Status` int(10) DEFAULT NULL,
  `Poster_Status` int(10) DEFAULT NULL,
  `Poster_DateSent` datetime DEFAULT NULL,
  `Poster_SentTries` tinyint(4) DEFAULT NULL,
  `Trigger_Type` tinyint(4) DEFAULT '-1',
  `IsDeleted` int(10) DEFAULT '-1',
  `Dummy_Status` int(10) DEFAULT '1000',
  PRIMARY KEY (`IdSchedule`),
  KEY `FK_IngestaSchedule` (`IdIngesta`),
  CONSTRAINT `FK_IngestaSchedule` FOREIGN KEY (`IdIngesta`) REFERENCES `mebs_ingesta` (`IdIngesta`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=13430 DEFAULT CHARSET=utf8 COMMENT='Trigger qui permettra de contrÃ´ler l''insertion dans la table Schedule,  avant chaque insertion, on change la valeur (IsPublished true ---> false) ';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_schedule`
--

LOCK TABLES `mebs_schedule` WRITE;
/*!40000 ALTER TABLE `mebs_schedule` DISABLE KEYS */;
/*!40000 ALTER TABLE `mebs_schedule` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_INSERT_MEBS_SCHEDULE`
BEFORE INSERT ON `mebs_schedule`
FOR EACH ROW
BEGIN
DECLARE TOTAL_RW INTEGER ;
DECLARE _ContentID INTEGER DEFAULT 0;
DECLARE min INTEGER DEFAULT 1;
DECLARE max INTEGER DEFAULT 65280;
DECLARE var_TimeBeforeLock VARCHAR(32);
DECLARE ContentType INTEGER DEFAULT -1;
DECLARE _CoverExtension VARCHAR(50) DEFAULT '';
DECLARE _NewcoverFileName VARCHAR(100) DEFAULT '';

/* Set the The LastNotificationDate &  LastNotificationAction before insertion of new schedule.*/
/*SELECT count(*) INTO TOTAL_RW FROM `mebs_schedule` WHERE IdIngesta = new.IdIngesta;*/

/*IF (TOTAL_RW = 0) THEN*/

update mebs_ingesta 
     SET IsPublished = true 
     WHERE IdIngesta= new.IdIngesta;  

update mebs_ingestadetails set DetailsValue =
(SELECT MediaFileSizeAfterRedundancy 
 FROM `mebs_ingesta` 
 where idingesta=new.IdIngesta) 
where 
IdIngesta=new.IdIngesta 
And 
DetailsName = 'contentFileSize';

/* Set LastNotificationDate_Lock DateTime UTC. */
SELECT Type INTO ContentType FROM `mebs_ingesta` WHERE idingesta = NEW.IdIngesta;
IF(ContentType = 0) THEN
	SELECT SettingValue INTO var_TimeBeforeLock FROM `mebs_settings` WHERE SettingName = 'AR_TimeBeforeLock'; -- AutoRecording
END IF;	
IF (ContentType > 0) THEN
	SELECT SettingValue INTO var_TimeBeforeLock FROM `mebs_settings` WHERE SettingName = 'DC_TimeBeforeLock'; -- DataCasting
END IF;	
IF(ContentType != -1) THEN
	IF(NEW.Estimated_Start <= DATE_ADD(UTC_TIMESTAMP(),INTERVAL var_TimeBeforeLock MINUTE)) THEN
			SET NEW.Status = 1007;

		 update mebs_settings 
		 SET SettingValue = UTC_TIMESTAMP()
		 WHERE SettingName= 'LastNotificationDate_Lock';

	END IF;
END IF;

/*New CODI-Id Generation*/
IF(new.EventId != NULL || new.EventId != ' ') THEN
	SELECT ContentID INTO _ContentID FROM `mebs_schedule` WHERE EventId = new.EventId LIMIT 1;
END IF;

IF (_ContentID = 0) THEN
	SELECT CEILING(RAND() * (max - min + 1)) + min - 1 INTO _ContentID;

END IF;

SET NEW.ContentID = _ContentID;

/* Update the coverFileName in mebs_ingestaDetails with ContentID.Extenxion */
-- Select Ingesta ConverName
SELECT PosterFileExtension INTO _CoverExtension FROM mebs_ingesta i WHERE i.Idingesta = new.Idingesta  LIMIT 1;

	IF(LENGTH(_CoverExtension) >0) THEN
		SET _NewcoverFileName = CONCAT_WS('',_ContentID,_CoverExtension);

		UPDATE mebs_ingestadetails id SET id.DetailsValue = _NewcoverFileName 
		WHERE  id.Idingesta = new.Idingesta AND id.DetailsName='coverFileName';
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_AFTER_INSERT_MEBS_SCHEDULE` AFTER INSERT ON `mebs_schedule` FOR EACH ROW
BEGIN
DECLARE _ContentType INTEGER DEFAULT -1;

-- Update the setting LastScheduleAddedOn by UTC time after each Datacast insertion.
SELECT Type INTO _ContentType FROM `mebs_ingesta` WHERE idingesta = NEW.IdIngesta;
IF(_ContentType = 1) THEN

	UPDATE mebs_settings 
	SET SettingValue = UTC_TIMESTAMP()
	WHERE SettingName= 'LastVideoContentScheduledOn';

END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_UPDATE_MEBS_SCHEDULE` AFTER UPDATE ON `mebs_schedule` FOR EACH ROW BEGIN

/* Set LastNotificationDate_Lock Date UTC */
IF(NEW.Status = 1007) THEN
	 
	UPDATE mebs_settings 
	SET SettingValue = UTC_TIMESTAMP()
	WHERE SettingName= 'LastNotificationDate_Lock';

END IF;


/* Set LastNotificationDate_Delete Date UTC*/
IF(NEW.IsDeleted = 0) THEN

	 UPDATE mebs_settings 
     SET SettingValue = UTC_TIMESTAMP()
     WHERE SettingName= 'LastNotificationDate_Delete';

END IF;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `TR_DELETE_MEBS_SCHEDULE` AFTER DELETE ON `mebs_schedule` FOR EACH ROW BEGIN
DECLARE _TOTAL_ROW INTEGER;
DECLARE _ContentType INTEGER DEFAULT -1;

SELECT COUNT(*) INTO _TOTAL_ROW FROM `mebs_schedule` WHERE IdIngesta = OLD.IdIngesta;
IF (_TOTAL_ROW = 0) THEN

	UPDATE mebs_ingesta 
	SET IsPublished = false 
	WHERE IdIngesta= OLD.IdIngesta;  
END IF;

-- Update the setting LastScheduleAddedOn by UTC time after each Datacast insertion.
SELECT Type INTO _ContentType FROM `mebs_ingesta` WHERE idingesta = OLD.IdIngesta;
IF(_ContentType = 1 && OLD.Estimated_Start > UTC_TIMESTAMP()) THEN

	UPDATE mebs_settings 
	SET SettingValue = UTC_TIMESTAMP()
	WHERE SettingName= 'LastVideoContentRemovedOn';

END IF;

END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `mebs_session`
--

DROP TABLE IF EXISTS `mebs_session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_session` (
  `SessionId` varchar(255) NOT NULL,
  `UserId` int(10) NOT NULL,
  `UserGuid` varchar(255) DEFAULT NULL,
  `LastAccess` datetime DEFAULT NULL,
  `IsExpired` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`SessionId`),
  KEY `session_FK_1_idx` (`UserId`),
  CONSTRAINT `session_FK_1` FOREIGN KEY (`UserId`) REFERENCES `mebs_login` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_session`
--

LOCK TABLES `mebs_session` WRITE;
/*!40000 ALTER TABLE `mebs_session` DISABLE KEYS */;
INSERT INTO `mebs_session` VALUES ('6ef21a98-bf04-4da7-9fd6-d2eb328d6ee9',14,'dd553500-8ae8-466c-89cf-315278088e3a','2014-05-05 11:13:14',0),('ea1c7069-6bef-4e6f-936a-707851e4df19',13,'4d362390-7e74-4ba4-9a2b-9922c9600280','2014-04-23 12:45:12',0);
/*!40000 ALTER TABLE `mebs_session` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_settings`
--

DROP TABLE IF EXISTS `mebs_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_settings` (
  `IdSetting` int(10) NOT NULL AUTO_INCREMENT,
  `SettingName` varchar(200) DEFAULT NULL,
  `SettingValue` varchar(200) DEFAULT NULL,
  `Description` text,
  `Visibility` varchar(1) NOT NULL DEFAULT 'N',
  PRIMARY KEY (`IdSetting`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_settings`
--

LOCK TABLES `mebs_settings` WRITE;
/*!40000 ALTER TABLE `mebs_settings` DISABLE KEYS */;
INSERT INTO `mebs_settings` VALUES (1,'BroadcasterID','513','The identifier of the broadcaster.','Y'),(2,'LastNotificationDate_Lock','2014-04-30 11:05:57',NULL,'N'),(3,'EventScheduler_SetLockValue_LastRun','2014-05-08 09:37:01',NULL,'N'),(4,'SchedulerTimeInterval','30',NULL,'Y'),(5,'DC_TimeBeforeLock','60',NULL,'Y'),(6,'DC_TimeInterval','16777215',NULL,'N'),(7,'DC_Inter_Package_Time_Gap','6',NULL,'Y'),(8,'DC_Time_Frame','5','','Y'),(9,'DC_User_Bitrate','3850000',NULL,'Y'),(10,'EventScheduler_SetIsExpiredValue_LastRun','2014-05-08 09:37:01',NULL,'N'),(11,'IngestedMedia_NLB_FTP_Relative_URI','/Contents/IngestedMedia/',NULL,'Y'),(12,'NLB_FTP_UserName','root',NULL,'Y'),(13,'NLB_FTP_UserPassword','televisio',NULL,'Y'),(14,'NLB_FTP_IPAddress','127.0.0.1','','Y'),(15,'DatacastCommand_Channel_Sending','0',NULL,'Y'),(16,'AR_Cover_Channel_Sending','1',NULL,'Y'),(17,'DC_Cover_Channel_Sending','D-check',NULL,'Y'),(18,'Interval_Cover_Sending','5',NULL,'Y'),(19,'DCCommand_Sending_Offset','-15','the offset to send the DCCommand','Y'),(20,'Categorization_Channel_Sending','0','','Y'),(21,'ExtendedAdv_Sending_Offset','D-Check','','Y'),(22,'DC_Inter_Command_Time_Gap','3',NULL,'Y'),(23,'Redundancy_BlockSize','558','Redundancy block Size (buffer size)','N'),(24,'Redundancy_RepetitionSpace','250','Redundancy repetition space','N'),(25,'Redundancy_RedundancyRate','0','Redundancy rate (Min 0|Max 100)','N'),(26,'LastVideoContentScheduledOn','2014-04-30 11:05:22',NULL,'N'),(27,'LastVideoContentRemovedOn','2014-04-14 12:41:09',NULL,'N'),(28,'FuturDCList_Channel_Sending','7','Channel used to send the Futur DC List XML','N');
/*!40000 ALTER TABLE `mebs_settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_useractivity`
--

DROP TABLE IF EXISTS `mebs_useractivity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_useractivity` (
  `ActivityLogID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `UserId` int(10) DEFAULT NULL,
  `Activity` varchar(255) DEFAULT NULL,
  `PageURL` varchar(255) DEFAULT NULL,
  `ActivityDate` datetime DEFAULT NULL,
  PRIMARY KEY (`ActivityLogID`),
  KEY `UserId` (`UserId`),
  KEY `useractivity_FK_1_idx` (`UserId`),
  CONSTRAINT `useractivity_FK_1` FOREIGN KEY (`UserId`) REFERENCES `mebs_login` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_useractivity`
--

LOCK TABLES `mebs_useractivity` WRITE;
/*!40000 ALTER TABLE `mebs_useractivity` DISABLE KEYS */;
/*!40000 ALTER TABLE `mebs_useractivity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_userdetails`
--

DROP TABLE IF EXISTS `mebs_userdetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_userdetails` (
  `UserDetailsID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `UserId` int(10) DEFAULT NULL,
  `Gender` varchar(255) DEFAULT NULL,
  `FirstName` varchar(255) DEFAULT NULL,
  `LastName` varchar(255) DEFAULT NULL,
  `Comment` varchar(255) DEFAULT NULL,
  `RegistrationDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `StreetAddress` varchar(255) DEFAULT NULL,
  `StreetAddress2` varchar(255) DEFAULT NULL,
  `PostalCode` varchar(255) DEFAULT NULL,
  `City` varchar(255) DEFAULT NULL,
  `Country` varchar(255) DEFAULT NULL,
  `Phone` varchar(255) DEFAULT NULL,
  `Mobile` varchar(255) DEFAULT NULL,
  `DateOfBirth` datetime DEFAULT NULL,
  `Picture` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`UserDetailsID`),
  KEY `UserId` (`UserId`),
  KEY `userdetails_FK_1_idx` (`UserId`),
  CONSTRAINT `userdetails_FK_1` FOREIGN KEY (`UserId`) REFERENCES `mebs_login` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_userdetails`
--

LOCK TABLES `mebs_userdetails` WRITE;
/*!40000 ALTER TABLE `mebs_userdetails` DISABLE KEYS */;
INSERT INTO `mebs_userdetails` VALUES (7,13,NULL,NULL,NULL,NULL,'2013-05-15 10:39:15',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),(8,14,'Female','Karima','goubar','No Comments for now !!!','2013-05-15 10:41:54','Mohammedia Rachidia','Mohammedia','20800','','Morocco','0651409697','',NULL,NULL);
/*!40000 ALTER TABLE `mebs_userdetails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mebs_usersinroles`
--

DROP TABLE IF EXISTS `mebs_usersinroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `mebs_usersinroles` (
  `IdUserRole` int(10) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(255) DEFAULT NULL,
  `RoleName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`IdUserRole`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mebs_usersinroles`
--

LOCK TABLES `mebs_usersinroles` WRITE;
/*!40000 ALTER TABLE `mebs_usersinroles` DISABLE KEYS */;
INSERT INTO `mebs_usersinroles` VALUES (10,'Administrator','MEBSAdmin'),(14,'user','MEBSMAM');
/*!40000 ALTER TABLE `mebs_usersinroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'sa_mebs'
--
/*!50003 DROP PROCEDURE IF EXISTS `mebs_Ingesta_SetIsExpiredValue` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `mebs_Ingesta_SetIsExpiredValue`()
BEGIN



 DECLARE done INT DEFAULT FALSE;

 DECLARE IngestaId INT;

 DECLARE cur1 CURSOR FOR SELECT idingesta from mebs_ingesta WHERE IsExpired = 0 and Expiration_time <= UTC_TIMESTAMP() and Expiration_time<> '1971-11-06 23:59:59';

 DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;



/* Event Scheduler Last Run on UTC. */

update mebs_settings set  SettingValue= UTC_TIMESTAMP()

where SettingName='EventScheduler_SetIsExpiredValue_LastRun';



 OPEN cur1;



 read_loop: LOOP

  IF done THEN

   LEAVE read_loop;

  END IF;



/* Set The IsExpired value to true (1). */

  FETCH cur1 INTO IngestaId;



	UPDATE mebs_ingesta SET IsExpired = 1

	WHERE idingesta = IngestaId;



 END LOOP;



close cur1;



END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `mebs_schedule_SetLockValue` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `mebs_schedule_SetLockValue`()
BEGIN



 DECLARE var_TimeBeforeLock VARCHAR(32);

 DECLARE done INT DEFAULT FALSE;

 DECLARE ScheduleId INT;

 DECLARE IngestaId INT;

 DECLARE EstimatedStart DATETIME;

 DECLARE EstimatedStop DATETIME;

 DECLARE ContentType INTEGER DEFAULT -1;

 DECLARE cur1 CURSOR FOR SELECT IdSchedule,idingesta,Estimated_Start,Estimated_Stop from mebs_schedule WHERE Status = 1000 order by Estimated_Start desc;

 DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;



/* Event Scheduler Last Run on UTC. */

update mebs_settings set  SettingValue= UTC_TIMESTAMP()

where SettingName='EventScheduler_SetLockValue_LastRun';



/*SELECT SettingValue INTO var_AR_TimeBeforeLock FROM `mebs_settings` WHERE SettingName = 'AR_TimeBeforeLock';*/



 OPEN cur1;



 read_loop: LOOP

  IF done THEN

   LEAVE read_loop;

  END IF;



/* Set LastNotificationDate_Lock DateTime UTC. */

  FETCH cur1 INTO ScheduleId,IngestaId,EstimatedStart,EstimatedStop;

	SELECT Type INTO ContentType FROM `mebs_ingesta` WHERE idingesta = IngestaId;

	IF(ContentType = 0) THEN

		SELECT SettingValue INTO var_TimeBeforeLock FROM `mebs_settings` WHERE SettingName = 'AR_TimeBeforeLock';

	END IF;	

	IF (ContentType > 0) THEN

		SELECT SettingValue INTO var_TimeBeforeLock FROM `mebs_settings` WHERE SettingName = 'DC_TimeBeforeLock';

	END IF;	

	IF(ContentType != -1) THEN

		IF(EstimatedStart <= DATE_ADD(UTC_TIMESTAMP(),INTERVAL var_TimeBeforeLock MINUTE)) THEN

			  UPDATE mebs_schedule SET Status = 1007 /* SET STATUS lOCKED*/

			  WHERE IdSchedule = ScheduleId;



			 update mebs_settings 

			 SET SettingValue = UTC_TIMESTAMP()

			 WHERE SettingName= 'LastNotificationDate_Lock';



			 /*update mebs_settings 

			 SET SettingValue = 'PUBLISH' 

			 WHERE SettingName= 'LastNotificationAction'; 	*/

		END IF;

	END IF;

 END LOOP;



close cur1;



/*IF(EstimatedStart <= DATE_ADD(UTC_TIMESTAMP(),INTERVAL var_AR_TimeBeforeLock MINUTE)) THEN

	  UPDATE mebs_schedule SET Status = 1007

	  WHERE IdSchedule = ScheduleId;

END IF;*/



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

-- Dump completed on 2014-05-08 11:20:31
