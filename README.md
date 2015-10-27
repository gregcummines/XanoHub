# XanoServiceNotificationCenter
XanoServiceNotificationCenter is a subscription and notification solution to decouple services from communicating with each other. It is implemented in VS2015 as a WCF service that is hosted in a Windows Service. 

WCF service to handle notifications and subscriptions between Xano subsystems
Example: 
1. Service 1 wants to send a FirmwareRelease notification to subsystems, so Service 1 creates the FirmwareRelease notification type.
2. Service 2 gets notifications and finds that there is a FirmwareRelease notification type. It is not necessary, but Service 2 could check the notification and find that Service 1 owns it. 
3. Service 2 subscribes to the FirmwareRelease notification and gives the SNC a Url to call. 
4. Service 1 handles a FirmwareRelease and notifies all subscribers (Service 2 is subscribed)
5. Service 2 wants to temporarily not listen to the FirmwareRelease notification, so Service 2 unsubscribes from that notification. 

Use: http://jsonschema.net/#/ to generate your schema.
Use: http://www.newtonsoft.com/json/help/html/JsonSchema.htm to validate the schema in C#