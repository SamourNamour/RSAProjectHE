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

	UPDATE mebs_category 
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

	UPDATE mebs_category 
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
-- Dumping data for table `mebs_schedule`
--
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

	UPDATE mebs_ingesta 
    SET IsPublished = true 
    WHERE IdIngesta= new.IdIngesta;  

	UPDATE mebs_ingestadetails set DetailsValue = (SELECT MediaFileSizeAfterRedundancy FROM `mebs_ingesta` WHERE idingesta=new.IdIngesta) 
	WHERE IdIngesta=new.IdIngesta 

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
		UPDATE mebs_settings 
		SET SettingValue = UTC_TIMESTAMP()
		WHERE SettingName= 'LastNotificationDate_Lock';

	END IF;

END IF;

/*New CODI-Id Generation*/

IF(NEW.EventId != NULL || NEW.EventId != ' ') THEN

	SELECT ContentID INTO _ContentID FROM `mebs_schedule` WHERE EventId = new.EventId LIMIT 1;

END IF;

IF (_ContentID = 0) THEN

	SELECT CEILING(RAND() * (max - min + 1)) + min - 1 INTO _ContentID;

END IF;

SET NEW.ContentID = _ContentID;

/* Update the coverFileName in mebs_ingestaDetails with ContentID.Extenxion */
-- Select Ingesta ConverName

SELECT PosterFileExtension INTO _CoverExtension FROM mebs_ingesta i WHERE i.Idingesta = NEW.Idingesta  LIMIT 1;

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
UPDATE mebs_settings SET  SettingValue= UTC_TIMESTAMP()
WHERE SettingName='EventScheduler_SetIsExpiredValue_LastRun';
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
UPDATE mebs_settings SET  SettingValue= UTC_TIMESTAMP()
WHERE SettingName='EventScheduler_SetLockValue_LastRun';

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

			  UPDATE mebs_settings 
			  SET SettingValue = UTC_TIMESTAMP()
			  WHERE SettingName= 'LastNotificationDate_Lock';

		END IF;
	END IF;

 END LOOP;

close cur1;

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
