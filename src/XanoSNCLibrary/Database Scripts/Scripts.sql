USE [XanoServiceNotificationCenter]
GO
/****** Object:  Table [dbo].[xNotification]    Script Date: 10/26/2015 12:49:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xNotification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PublisherId] [int] NOT NULL,
	[NotificationEventId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[xNotificationEvent]    Script Date: 10/26/2015 12:49:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xNotificationEvent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[PublisherId] [int] NOT NULL,
	[JsonSchema] [nvarchar](4000) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_NotificationEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[xPublisher]    Script Date: 10/26/2015 12:49:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xPublisher](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Publisher] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[xSubscriber]    Script Date: 10/26/2015 12:49:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xSubscriber](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Subscriber] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[xSubscription]    Script Date: 10/26/2015 12:49:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xSubscription](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SubscriberId] [int] NOT NULL,
	[NotificationEventId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[NotifyURL] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[xSubscriptionNotifications]    Script Date: 10/26/2015 12:49:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xSubscriptionNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[NotificationId] [int] NOT NULL,
	[NotificationError] [nvarchar](4000) NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_SubscriptionNotifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[xNotification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_NotificationEvent] FOREIGN KEY([NotificationEventId])
REFERENCES [dbo].[xNotificationEvent] ([Id])
GO
ALTER TABLE [dbo].[xNotification] CHECK CONSTRAINT [FK_Notification_NotificationEvent]
GO
ALTER TABLE [dbo].[xNotification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Publisher] FOREIGN KEY([PublisherId])
REFERENCES [dbo].[xPublisher] ([Id])
GO
ALTER TABLE [dbo].[xNotification] CHECK CONSTRAINT [FK_Notification_Publisher]
GO
ALTER TABLE [dbo].[xNotificationEvent]  WITH CHECK ADD  CONSTRAINT [FK_NotificationEvent_Publisher] FOREIGN KEY([PublisherId])
REFERENCES [dbo].[xPublisher] ([Id])
GO
ALTER TABLE [dbo].[xNotificationEvent] CHECK CONSTRAINT [FK_NotificationEvent_Publisher]
GO
ALTER TABLE [dbo].[xSubscription]  WITH CHECK ADD  CONSTRAINT [FK_Subscription_NotificationEvent] FOREIGN KEY([NotificationEventId])
REFERENCES [dbo].[xNotificationEvent] ([Id])
GO
ALTER TABLE [dbo].[xSubscription] CHECK CONSTRAINT [FK_Subscription_NotificationEvent]
GO
ALTER TABLE [dbo].[xSubscription]  WITH CHECK ADD  CONSTRAINT [FK_Subscription_Subscriber] FOREIGN KEY([SubscriberId])
REFERENCES [dbo].[xSubscriber] ([Id])
GO
ALTER TABLE [dbo].[xSubscription] CHECK CONSTRAINT [FK_Subscription_Subscriber]
GO
ALTER TABLE [dbo].[xSubscriptionNotifications]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionNotifications_Notification] FOREIGN KEY([NotificationId])
REFERENCES [dbo].[xNotification] ([Id])
GO
ALTER TABLE [dbo].[xSubscriptionNotifications] CHECK CONSTRAINT [FK_SubscriptionNotifications_Notification]
GO
ALTER TABLE [dbo].[xSubscriptionNotifications]  WITH CHECK ADD  CONSTRAINT [FK_SubscriptionNotifications_Subscription] FOREIGN KEY([SubscriptionId])
REFERENCES [dbo].[xSubscription] ([Id])
GO
ALTER TABLE [dbo].[xSubscriptionNotifications] CHECK CONSTRAINT [FK_SubscriptionNotifications_Subscription]
GO
