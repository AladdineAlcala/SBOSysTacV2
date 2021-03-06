USE [master]
GO
/****** Object:  Database [PegasusTacV2]    Script Date: 10/20/2021 9:30:20 PM ******/
CREATE DATABASE [PegasusTacV2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PegasusTacV2
', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\PegasusTacV2.mdf' , SIZE = 16384KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PegasusTacV2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\PegasusTacV2_log.ldf' , SIZE = 32448KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PegasusTacV2] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PegasusTacV2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PegasusTacV2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PegasusTacV2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PegasusTacV2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PegasusTacV2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PegasusTacV2] SET ARITHABORT OFF 
GO
ALTER DATABASE [PegasusTacV2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PegasusTacV2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PegasusTacV2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PegasusTacV2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PegasusTacV2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PegasusTacV2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PegasusTacV2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PegasusTacV2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PegasusTacV2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PegasusTacV2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PegasusTacV2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PegasusTacV2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PegasusTacV2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PegasusTacV2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PegasusTacV2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PegasusTacV2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PegasusTacV2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PegasusTacV2] SET RECOVERY FULL 
GO
ALTER DATABASE [PegasusTacV2] SET  MULTI_USER 
GO
ALTER DATABASE [PegasusTacV2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PegasusTacV2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PegasusTacV2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PegasusTacV2] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [PegasusTacV2] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PegasusTacV2', N'ON'
GO
USE [PegasusTacV2]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AddonCategory]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddonCategory](
	[addoncatId] [int] IDENTITY(1,1) NOT NULL,
	[addoncatdesc] [nvarchar](50) NULL,
 CONSTRAINT [PK_AddonCategory] PRIMARY KEY CLUSTERED 
(
	[addoncatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AddonDetails]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddonDetails](
	[addonId] [int] IDENTITY(1,1) NOT NULL,
	[addoncatId] [int] NULL,
	[deptId] [int] NULL,
	[addondescription] [nvarchar](50) NULL,
	[unit] [nvarchar](25) NULL,
	[amount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_AddonDetails] PRIMARY KEY CLUSTERED 
(
	[addonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Areas]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Areas](
	[aID] [int] IDENTITY(1,1) NOT NULL,
	[AreaDetails] [nvarchar](50) NULL,
 CONSTRAINT [PK_Areas] PRIMARY KEY CLUSTERED 
(
	[aID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AuditLogDetails]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](50) NULL,
	[OriginalValue] [nvarchar](50) NULL,
	[NewValue] [nvarchar](50) NULL,
	[AuditLogId] [int] NULL,
 CONSTRAINT [PK_AuditLogDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[AuditLogId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[EventDateUTC] [datetime] NULL,
	[AuditOperation] [nvarchar](50) NULL,
	[TableName] [nvarchar](50) NULL,
	[AuditData] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED 
(
	[AuditLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Book_Discount]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book_Discount](
	[discno] [int] IDENTITY(1,1) NOT NULL,
	[trn_Id] [int] NULL,
	[disc_Id] [int] NULL,
	[userid] [nvarchar](50) NULL,
 CONSTRAINT [PK_Book_Discount] PRIMARY KEY CLUSTERED 
(
	[discno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Book_Menus]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book_Menus](
	[No] [int] IDENTITY(1,1) NOT NULL,
	[trn_Id] [int] NULL,
	[menuid] [nvarchar](20) NULL,
	[serving] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Book_Menus] PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BookingAddons]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingAddons](
	[No] [int] IDENTITY(1,1) NOT NULL,
	[trn_Id] [int] NULL,
	[addonId] [int] NULL,
	[Addondesc] [nvarchar](50) NULL,
	[Note] [nvarchar](50) NULL,
	[addonQty] [decimal](18, 0) NULL,
	[AddonAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_BookingAddons] PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Bookings]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookings](
	[trn_Id] [int] IDENTITY(1,1) NOT NULL,
	[transdate] [datetime] NULL,
	[booktype] [nchar](10) NULL,
	[c_Id] [int] NULL,
	[noofperson] [int] NULL,
	[occasion] [nvarchar](50) NULL,
	[venue] [nvarchar](50) NULL,
	[typeofservice] [int] NULL,
	[startdate] [datetime] NULL,
	[enddate] [datetime] NULL,
	[eventcolor] [nvarchar](25) NULL,
	[p_id] [int] NULL,
	[reference] [nvarchar](max) NULL,
	[extendedAreaId] [int] NULL,
	[apply_extendedAmount] [bit] NULL CONSTRAINT [DF_Bookings_apply_extendedAmount]  DEFAULT ((0)),
	[p_amount] [decimal](18, 2) NULL,
	[serve_stat] [bit] NULL CONSTRAINT [DF_Bookings_status]  DEFAULT ((0)),
	[is_cancelled] [bit] NULL CONSTRAINT [DF_Bookings_is_cancelled]  DEFAULT ((0)),
	[b_createdbyUser] [nvarchar](128) NOT NULL,
	[b_updatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Bookings] PRIMARY KEY CLUSTERED 
(
	[trn_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CancelledBookings]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CancelledBookings](
	[cNo] [bigint] IDENTITY(1,1) NOT NULL,
	[cancelledDated] [datetime] NULL,
	[trn_Id] [int] NULL,
	[reasoncancelled] [nvarchar](max) NULL,
	[isrefundable] [bit] NULL,
 CONSTRAINT [PK_CancelledBookings] PRIMARY KEY CLUSTERED 
(
	[cNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CateringDiscounts]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CateringDiscounts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[DiscPaxMin] [int] NULL,
	[DiscPaxMax] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_CateringDiscounts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CourseCategory]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseCategory](
	[CourserId] [int] IDENTITY(1,1) NOT NULL,
	[Course] [nvarchar](50) NULL,
	[Note] [nvarchar](50) NULL,
	[Main_Bol] [bit] NULL,
 CONSTRAINT [PK_CourseCategory] PRIMARY KEY CLUSTERED 
(
	[CourserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[c_Id] [int] IDENTITY(1,1) NOT NULL,
	[lastname] [nvarchar](50) NULL,
	[firstname] [nvarchar](50) NULL,
	[middle] [nchar](10) NULL,
	[address] [nvarchar](50) NULL,
	[contact1] [nvarchar](30) NULL,
	[contact2] [nvarchar](30) NULL,
	[datereg] [datetime] NULL,
	[company] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[c_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Department]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[deptId] [int] IDENTITY(1,1) NOT NULL,
	[deptName] [nvarchar](30) NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[deptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Discount]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount](
	[disc_Id] [int] IDENTITY(1,1) NOT NULL,
	[discCode] [nvarchar](50) NULL,
	[disctype] [nchar](10) NULL,
	[discount] [decimal](18, 2) NULL,
	[discStartdate] [datetime] NULL,
	[discEnddate] [datetime] NULL,
 CONSTRAINT [PK_Discount] PRIMARY KEY CLUSTERED 
(
	[disc_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menu]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[menuid] [nvarchar](20) NOT NULL,
	[CourserId] [int] NULL,
	[menu_name] [nvarchar](50) NULL,
	[deptId] [int] NULL,
	[note] [nvarchar](max) NULL,
	[image] [nvarchar](50) NULL,
	[date_added] [datetime] NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[menuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PackageAreaCoverage]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageAreaCoverage](
	[p_areaNo] [int] IDENTITY(1,1) NOT NULL,
	[p_id] [int] NULL,
	[aID] [int] NULL,
	[is_extended] [bit] NULL,
	[ext_amount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PackageAreaCoverage] PRIMARY KEY CLUSTERED 
(
	[p_areaNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PackageBody]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageBody](
	[No] [int] IDENTITY(1,1) NOT NULL,
	[p_id] [int] NULL,
	[courseId] [int] NULL,
 CONSTRAINT [PK_PackageBody] PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Packages]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Packages](
	[p_id] [int] IDENTITY(1,1) NOT NULL,
	[p_type] [nchar](10) NULL,
	[p_descripton] [nvarchar](max) NULL,
	[p_amountPax] [decimal](18, 2) NULL,
	[nopax_id] [int] NULL,
	[p_min] [int] NULL,
	[isActive] [bit] NOT NULL CONSTRAINT [DF_Packages_discon]  DEFAULT ((1)),
 CONSTRAINT [PK_Packages] PRIMARY KEY CLUSTERED 
(
	[p_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Packages_No_Pax_applicable]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Packages_No_Pax_applicable](
	[nopax_id] [int] IDENTITY(1,1) NOT NULL,
	[package_n_pax] [nvarchar](50) NULL,
 CONSTRAINT [PK_Packages_No_Pax_applicable] PRIMARY KEY CLUSTERED 
(
	[nopax_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PackagesRangeBelowMin]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackagesRangeBelowMin](
	[no] [int] IDENTITY(1,1) NOT NULL,
	[pMin] [int] NULL,
	[pMax] [int] NULL,
	[Amt_added] [decimal](18, 2) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Payments]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[payNo] [nvarchar](50) NOT NULL,
	[trn_Id] [int] NULL,
	[dateofPayment] [datetime] NULL,
	[particular] [nvarchar](50) NULL,
	[payType] [int] NULL,
	[amtPay] [decimal](18, 2) NULL,
	[pay_means] [nchar](10) NULL,
	[checkNo] [nvarchar](50) NULL,
	[notes] [nvarchar](50) NULL,
	[p_createdbyUser] [nvarchar](50) NULL,
 CONSTRAINT [PK_Payments_1] PRIMARY KEY CLUSTERED 
(
	[payNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RefundEntry]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefundEntry](
	[No] [bigint] IDENTITY(1,1) NOT NULL,
	[Rf_id] [bigint] NULL,
	[Particular] [nvarchar](max) NULL,
	[Amount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_RefundEntry] PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Refunds]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Refunds](
	[Rf_id] [bigint] IDENTITY(1,1) NOT NULL,
	[rfDate] [datetime] NULL,
	[trn_Id] [int] NULL,
	[rf_Reason] [nvarchar](max) NULL,
	[rf_Amount] [decimal](18, 2) NULL,
	[rfDeduction] [decimal](18, 2) NULL,
	[rfNetAmount] [decimal](18, 2) NULL,
	[rf_Stat] [int] NULL,
 CONSTRAINT [PK_Refunds] PRIMARY KEY CLUSTERED 
(
	[Rf_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Reservations]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservations](
	[resId] [int] IDENTITY(1,1) NOT NULL,
	[c_Id] [int] NOT NULL,
	[resDate] [datetime] NOT NULL,
	[noofPax] [int] NOT NULL,
	[occasion] [nvarchar](50) NULL,
	[reserveStat] [bit] NOT NULL CONSTRAINT [DF_Reservations_reserveStat]  DEFAULT ((1)),
	[eventVenue] [nvarchar](50) NULL,
 CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED 
(
	[resId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceType]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceType](
	[serviceId] [int] IDENTITY(1,1) NOT NULL,
	[servicetypedetails] [nvarchar](30) NULL,
 CONSTRAINT [PK_ServiceType] PRIMARY KEY CLUSTERED 
(
	[serviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.UserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserLogins]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.UserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[No] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[Firstname] [nvarchar](50) NULL,
	[Lastname] [nvarchar](50) NULL,
	[DofB] [datetime] NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](128) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Firstname] [nvarchar](50) NULL,
	[Lastname] [nvarchar](50) NULL,
	[Middle] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[countMenusOrder]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[countMenusOrder]
AS
SELECT        dbo.Book_Menus.menuid, dbo.CourseCategory.Course, dbo.Menu.menu_name, COUNT(dbo.Menu.menu_name) AS countOrder
FROM            dbo.Book_Menus INNER JOIN
                         dbo.Menu ON dbo.Book_Menus.menuid = dbo.Menu.menuid INNER JOIN
                         dbo.CourseCategory ON dbo.Menu.CourserId = dbo.CourseCategory.CourserId
GROUP BY dbo.Book_Menus.menuid, dbo.CourseCategory.Course, dbo.Menu.menu_name



GO
/****** Object:  View [dbo].[CourseCount]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CourseCount]
AS
SELECT        dbo.Book_Menus.trn_Id, dbo.CourseCategory.CourserId, COUNT(dbo.CourseCategory.CourserId) AS Coursecount
FROM            dbo.Book_Menus INNER JOIN
                         dbo.Menu ON dbo.Book_Menus.menuid = dbo.Menu.menuid INNER JOIN
                         dbo.CourseCategory ON dbo.Menu.CourserId = dbo.CourseCategory.CourserId
GROUP BY dbo.Book_Menus.trn_Id, dbo.CourseCategory.CourserId



GO
/****** Object:  View [dbo].[View_1]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_1]
AS
SELECT        dbo.PackageAreaCoverage.aID, dbo.Areas.AreaDetails
FROM            dbo.PackageAreaCoverage INNER JOIN
                         dbo.Areas ON dbo.PackageAreaCoverage.aID = dbo.Areas.aID
GROUP BY dbo.PackageAreaCoverage.aID, dbo.Areas.AreaDetails



GO
/****** Object:  View [dbo].[View_3]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_3]
AS
SELECT        dbo.Book_Menus.trn_Id, dbo.CourseCategory.CourserId
FROM            dbo.Book_Menus INNER JOIN
                         dbo.Menu ON dbo.Book_Menus.menuid = dbo.Menu.menuid INNER JOIN
                         dbo.CourseCategory ON dbo.Menu.CourserId = dbo.CourseCategory.CourserId
WHERE        (dbo.Book_Menus.trn_Id = 29)



GO
/****** Object:  View [dbo].[View_4]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_4]
AS
SELECT        dbo.Book_Menus.menuid, dbo.Menu.menu_name, dbo.Book_Menus.trn_Id
FROM            dbo.Book_Menus INNER JOIN
                         dbo.Menu ON dbo.Book_Menus.menuid = dbo.Menu.menuid
WHERE        (dbo.Book_Menus.menuid = N'0004')



GO
/****** Object:  Index [IX_Customer]    Script Date: 10/20/2021 9:30:20 PM ******/
CREATE NONCLUSTERED INDEX [IX_Customer] ON [dbo].[Customer]
(
	[c_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AddonDetails]  WITH CHECK ADD  CONSTRAINT [FK_AddonDetails_AddonCategory] FOREIGN KEY([addoncatId])
REFERENCES [dbo].[AddonCategory] ([addoncatId])
GO
ALTER TABLE [dbo].[AddonDetails] CHECK CONSTRAINT [FK_AddonDetails_AddonCategory]
GO
ALTER TABLE [dbo].[AddonDetails]  WITH CHECK ADD  CONSTRAINT [FK_AddonDetails_Department] FOREIGN KEY([deptId])
REFERENCES [dbo].[Department] ([deptId])
GO
ALTER TABLE [dbo].[AddonDetails] CHECK CONSTRAINT [FK_AddonDetails_Department]
GO
ALTER TABLE [dbo].[AuditLogDetails]  WITH CHECK ADD  CONSTRAINT [FK_AuditLogDetails_AuditLogs] FOREIGN KEY([AuditLogId])
REFERENCES [dbo].[AuditLogs] ([AuditLogId])
GO
ALTER TABLE [dbo].[AuditLogDetails] CHECK CONSTRAINT [FK_AuditLogDetails_AuditLogs]
GO
ALTER TABLE [dbo].[Book_Discount]  WITH CHECK ADD  CONSTRAINT [FK_Book_Discount_Bookings] FOREIGN KEY([trn_Id])
REFERENCES [dbo].[Bookings] ([trn_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Book_Discount] CHECK CONSTRAINT [FK_Book_Discount_Bookings]
GO
ALTER TABLE [dbo].[Book_Discount]  WITH CHECK ADD  CONSTRAINT [FK_Book_Discount_Discount] FOREIGN KEY([disc_Id])
REFERENCES [dbo].[Discount] ([disc_Id])
GO
ALTER TABLE [dbo].[Book_Discount] CHECK CONSTRAINT [FK_Book_Discount_Discount]
GO
ALTER TABLE [dbo].[Book_Menus]  WITH CHECK ADD  CONSTRAINT [FK_Book_Menus_Bookings] FOREIGN KEY([trn_Id])
REFERENCES [dbo].[Bookings] ([trn_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Book_Menus] CHECK CONSTRAINT [FK_Book_Menus_Bookings]
GO
ALTER TABLE [dbo].[Book_Menus]  WITH CHECK ADD  CONSTRAINT [FK_Book_Menus_Menu] FOREIGN KEY([menuid])
REFERENCES [dbo].[Menu] ([menuid])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Book_Menus] CHECK CONSTRAINT [FK_Book_Menus_Menu]
GO
ALTER TABLE [dbo].[BookingAddons]  WITH CHECK ADD  CONSTRAINT [FK_BookingAddons_Bookings] FOREIGN KEY([trn_Id])
REFERENCES [dbo].[Bookings] ([trn_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookingAddons] CHECK CONSTRAINT [FK_BookingAddons_Bookings]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Customer] FOREIGN KEY([c_Id])
REFERENCES [dbo].[Customer] ([c_Id])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_Customer]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Packages] FOREIGN KEY([p_id])
REFERENCES [dbo].[Packages] ([p_id])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_Packages]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_ServiceType] FOREIGN KEY([typeofservice])
REFERENCES [dbo].[ServiceType] ([serviceId])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_ServiceType]
GO
ALTER TABLE [dbo].[CancelledBookings]  WITH CHECK ADD  CONSTRAINT [FK_CancelledBookings_Bookings] FOREIGN KEY([trn_Id])
REFERENCES [dbo].[Bookings] ([trn_Id])
GO
ALTER TABLE [dbo].[CancelledBookings] CHECK CONSTRAINT [FK_CancelledBookings_Bookings]
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_CourseCategory] FOREIGN KEY([CourserId])
REFERENCES [dbo].[CourseCategory] ([CourserId])
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_Menu_CourseCategory]
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_Department] FOREIGN KEY([deptId])
REFERENCES [dbo].[Department] ([deptId])
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_Menu_Department]
GO
ALTER TABLE [dbo].[PackageAreaCoverage]  WITH CHECK ADD  CONSTRAINT [FK_PackageAreaCoverage_Areas] FOREIGN KEY([aID])
REFERENCES [dbo].[Areas] ([aID])
GO
ALTER TABLE [dbo].[PackageAreaCoverage] CHECK CONSTRAINT [FK_PackageAreaCoverage_Areas]
GO
ALTER TABLE [dbo].[PackageAreaCoverage]  WITH CHECK ADD  CONSTRAINT [FK_PackageAreaCoverage_Packages] FOREIGN KEY([p_id])
REFERENCES [dbo].[Packages] ([p_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageAreaCoverage] CHECK CONSTRAINT [FK_PackageAreaCoverage_Packages]
GO
ALTER TABLE [dbo].[PackageBody]  WITH CHECK ADD  CONSTRAINT [FK_PackageBody_Packages] FOREIGN KEY([p_id])
REFERENCES [dbo].[Packages] ([p_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageBody] CHECK CONSTRAINT [FK_PackageBody_Packages]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Bookings] FOREIGN KEY([trn_Id])
REFERENCES [dbo].[Bookings] ([trn_Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Bookings]
GO
ALTER TABLE [dbo].[RefundEntry]  WITH CHECK ADD  CONSTRAINT [FK_RefundEntry_Refunds] FOREIGN KEY([Rf_id])
REFERENCES [dbo].[Refunds] ([Rf_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RefundEntry] CHECK CONSTRAINT [FK_RefundEntry_Refunds]
GO
ALTER TABLE [dbo].[Refunds]  WITH CHECK ADD  CONSTRAINT [FK_Refunds_Bookings] FOREIGN KEY([trn_Id])
REFERENCES [dbo].[Bookings] ([trn_Id])
GO
ALTER TABLE [dbo].[Refunds] CHECK CONSTRAINT [FK_Refunds_Bookings]
GO
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD  CONSTRAINT [FK_Reservations_Customer] FOREIGN KEY([c_Id])
REFERENCES [dbo].[Customer] ([c_Id])
GO
ALTER TABLE [dbo].[Reservations] CHECK CONSTRAINT [FK_Reservations_Customer]
GO
ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[UserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserLogins_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserLogins] CHECK CONSTRAINT [FK_dbo.UserLogins_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRoles_dbo.Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_dbo.UserRoles_dbo.Roles_RoleId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRoles_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_dbo.UserRoles_dbo.Users_UserId]
GO
/****** Object:  StoredProcedure [dbo].[Generate_EventSlip]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generate_EventSlip]
	
	(@series INT OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select @series=(SELECT CAST(RIGHT(MAX(particular), 4) AS INT)FROM dbo.Payments)
	SET @series=@series+1
	select @series

END



GO
/****** Object:  StoredProcedure [dbo].[Generate_MenuCode]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generate_MenuCode]
	
	(@series INT OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select @series=(SELECT CAST(RIGHT(MAX(menuid), 4) AS INT)FROM dbo.Menu)
	SET @series=@series+1
	select @series

END



GO
/****** Object:  StoredProcedure [dbo].[Generate_PmtCode]    Script Date: 10/20/2021 9:30:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generate_PmtCode]
	-- Add the parameters for the stored procedure here

	(@series INT OUTPUT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select @series=(SELECT CAST(RIGHT(MAX(payNo),7) AS INT)FROM dbo.Payments)
	SET @series=@series+1
	select @series
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[37] 4[22] 2[13] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Book_Menus"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 211
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Menu"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 211
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CourseCategory"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 2265
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'countMenusOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'countMenusOrder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[48] 4[21] 2[5] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Book_Menus"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 209
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Menu"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 227
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CourseCategory"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 224
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 5655
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CourseCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CourseCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[21] 2[17] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PackageAreaCoverage"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 211
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Areas"
            Begin Extent = 
               Top = 14
               Left = 357
               Bottom = 166
               Right = 527
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[11] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Book_Menus"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 211
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CourseCategory"
            Begin Extent = 
               Top = 0
               Left = 610
               Bottom = 211
               Right = 785
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Menu"
            Begin Extent = 
               Top = 0
               Left = 435
               Bottom = 211
               Right = 604
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[21] 2[14] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Book_Menus"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 210
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Menu"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 211
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_4'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_4'
GO
USE [master]
GO
ALTER DATABASE [PegasusTacV2] SET  READ_WRITE 
GO
