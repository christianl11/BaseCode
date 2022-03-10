/*
SQLyog Community v13.1.9 (64 bit)
MySQL - 10.4.8-MariaDB : Database - BASE
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`BASE` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;

USE `BASE`;

/*Table structure for table `MODULE` */

DROP TABLE IF EXISTS `MODULE`;

CREATE TABLE `MODULE` (
  `MODULE_ID` int(11) NOT NULL AUTO_INCREMENT,
  `NAME` varchar(50) DEFAULT NULL,
  `DESCRIPTION` varchar(50) DEFAULT NULL,
  `STATUS` char(1) DEFAULT 'A',
  `CREATE_DATE` datetime DEFAULT current_timestamp(),
  `MODULE_UPDATE_DATE` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`MODULE_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=143 DEFAULT CHARSET=utf8mb4;

/*Data for the table `MODULE` */

insert  into `MODULE`(`MODULE_ID`,`NAME`,`DESCRIPTION`,`STATUS`,`CREATE_DATE`,`MODULE_UPDATE_DATE`) values 
(140,'UeFftffa',' Prog','A','2022-03-09 10:12:07','2022-03-09 10:12:07'),
(141,'Login',' Prog','A','2022-03-09 10:47:12','2022-03-09 10:47:12'),
(142,'Logon',' Prog','A','2022-03-09 11:07:27','2022-03-09 11:07:27');

/*Table structure for table `USER` */

DROP TABLE IF EXISTS `USER`;

CREATE TABLE `USER` (
  `USER_ID` int(11) NOT NULL AUTO_INCREMENT,
  `FIRST_NAME` varchar(50) DEFAULT NULL,
  `LAST_NAME` varchar(50) DEFAULT NULL,
  `USER_NAME` varchar(50) DEFAULT NULL,
  `PASSWORD` varchar(80) DEFAULT NULL,
  `STATUS` char(1) DEFAULT 'A',
  `CREATE_DATE` datetime DEFAULT current_timestamp(),
  `UPDATE_DATE` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `EMAIL_ADDRESS` varchar(50) DEFAULT NULL,
  `USER_TYPE_ID` int(11) DEFAULT NULL,
  PRIMARY KEY (`USER_ID`),
  KEY `user_ibfk_1` (`USER_TYPE_ID`),
  CONSTRAINT `user_ibfk_1` FOREIGN KEY (`USER_TYPE_ID`) REFERENCES `user_types` (`USER_TYPE_ID`) ON DELETE CASCADE ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4;

/*Data for the table `USER` */

insert  into `USER`(`USER_ID`,`FIRST_NAME`,`LAST_NAME`,`USER_NAME`,`PASSWORD`,`STATUS`,`CREATE_DATE`,`UPDATE_DATE`,`EMAIL_ADDRESS`,`USER_TYPE_ID`) values 
(47,'Christiann','Ortiz','BitonyyyyYT','Abc123','A','2022-03-09 10:09:56','2022-03-09 11:03:42','christianlacaba@gmail.com',27),
(49,'Chrisian','Ortiz','Superbrr','Abc123','A','2022-03-09 10:29:32','2022-03-09 10:54:15','christianlacaba@gmail.com',26),
(52,'Chrisian','Ortiz','Superbrrr','Abc123','A','2022-03-09 11:06:28','2022-03-09 11:06:28','christianlacaba@gmail.com',28);

/*Table structure for table `USER_TYPES` */

DROP TABLE IF EXISTS `USER_TYPES`;

CREATE TABLE `USER_TYPES` (
  `USER_TYPE_ID` int(11) NOT NULL AUTO_INCREMENT,
  `USER_TYPE_DESCRIPTION` varchar(80) DEFAULT NULL,
  `STATUS` char(1) DEFAULT 'A',
  `CREATE_DATE` datetime DEFAULT current_timestamp(),
  `UPDATE_DATE` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`USER_TYPE_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4;

/*Data for the table `USER_TYPES` */

insert  into `USER_TYPES`(`USER_TYPE_ID`,`USER_TYPE_DESCRIPTION`,`STATUS`,`CREATE_DATE`,`UPDATE_DATE`) values 
(26,'Admin','A','2022-03-09 10:46:19','2022-03-09 10:46:19'),
(27,'BITOY','A','2022-03-09 10:54:40','2022-03-09 11:05:55'),
(28,'Admin','A','2022-03-09 11:04:53','2022-03-09 11:04:53');

/*Table structure for table `USER_TYPE_MODULE_ACCESS` */

DROP TABLE IF EXISTS `USER_TYPE_MODULE_ACCESS`;

CREATE TABLE `USER_TYPE_MODULE_ACCESS` (
  `UTMA_ID` int(11) NOT NULL AUTO_INCREMENT,
  `MODULE_ID` int(11) DEFAULT NULL,
  `USER_TYPE_ID` int(11) DEFAULT NULL,
  `STATUS` char(1) DEFAULT 'A',
  `CREATE_DATE` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`UTMA_ID`),
  KEY `user_type_module_access_ibfk_1` (`MODULE_ID`),
  KEY `user_type_module_access_ibfk_2` (`USER_TYPE_ID`),
  CONSTRAINT `user_type_module_access_ibfk_1` FOREIGN KEY (`MODULE_ID`) REFERENCES `module` (`MODULE_ID`) ON DELETE CASCADE ON UPDATE SET NULL,
  CONSTRAINT `user_type_module_access_ibfk_2` FOREIGN KEY (`USER_TYPE_ID`) REFERENCES `user_types` (`USER_TYPE_ID`) ON DELETE CASCADE ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4;

/*Data for the table `USER_TYPE_MODULE_ACCESS` */

insert  into `USER_TYPE_MODULE_ACCESS`(`UTMA_ID`,`MODULE_ID`,`USER_TYPE_ID`,`STATUS`,`CREATE_DATE`) values 
(24,140,26,'A','2022-03-09 10:12:12'),
(25,141,27,'A','2022-03-09 10:46:56'),
(28,141,28,'A','2022-03-09 11:08:28');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
