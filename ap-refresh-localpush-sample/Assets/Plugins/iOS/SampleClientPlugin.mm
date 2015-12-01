extern "C" void UnitySendMessage(const char *, const char *, const char *);

@interface SampleClientPlugin : NSObject
{
    NSString *gameObjectName;
}
@end

@implementation SampleClientPlugin

NSString* NSStringFromCharString(const char* inStr)
{
    NSString* str = [NSString stringWithCString:inStr encoding:NSUTF8StringEncoding];
    return str;
}

- (id)initWithGameObjectName:(const char *)gameObjectName_
{
    self = [super init];
    
    gameObjectName = [[NSString stringWithUTF8String:gameObjectName_] retain];
    
    return self;
}

- (void)dealloc
{
    [gameObjectName release];
    [super dealloc];
}

- (void) setLocalNotification:(int) notificationId title:(const char *)title msg:(const char *)msg interval:(int) interval
{
    NSLog(@"setLocalNotification:%d %d", notificationId, interval);
    
    [self cancelLocalNotification:notificationId];
    
    UILocalNotification *notification = [[[UILocalNotification alloc] init] autorelease];
    notification.fireDate = [NSDate dateWithTimeIntervalSinceNow:(interval)];
    notification.timeZone = [NSTimeZone defaultTimeZone];
    notification.alertBody = NSStringFromCharString(msg);
    notification.applicationIconBadgeNumber = 1;
    notification.userInfo = [NSDictionary dictionaryWithObject:[NSString stringWithFormat:@"%d", notificationId] forKey:@"NOTIFICATION_ID"];
    
    [[UIApplication sharedApplication] scheduleLocalNotification:notification];
}

- (void) cancelLocalNotification:(int) notificationId
{
    NSLog(@"cancelLocalNotification:%d", notificationId);
    
    UIApplication* app = [UIApplication sharedApplication];
    for(UILocalNotification *notification in app.scheduledLocalNotifications) {
        NSLog(@"notification %d", [[notification.userInfo objectForKey:@"NOTIFICATION_ID"] integerValue]);
        if([[notification.userInfo objectForKey:@"NOTIFICATION_ID"] integerValue] == notificationId) {
            NSLog(@"cancelLocalNotification!!");
            [app cancelLocalNotification:notification];
        }
    }
    
    app.applicationIconBadgeNumber = -1;
}

@end

extern "C" {
    void *_SampleClientPlugin_Init(const char *gameObjectName);
    void _SampleClientPlugin_Destroy(void *instance);
    void _SampleClientPlugin_SetLocalNotification(void *instance, int notificationId, const char *title, const char *msg, int interval);
    void _SampleClientPlugin_CancelLocalNotification(void *instance, int notificationId);}

void *_SampleClientPlugin_Init(const char *gameObjectName)
{
    id instance = [[SampleClientPlugin alloc] initWithGameObjectName:gameObjectName];
    return (void *)instance;
}

void _SampleClientPlugin_Destroy(void *instance)
{
    SampleClientPlugin *sampleClientPlugin = (SampleClientPlugin *)instance;
    [sampleClientPlugin release];
}

void _SampleClientPlugin_SetLocalNotification(void *instance, int notificationId, const char *title, const char *msg, int interval)
{
    SampleClientPlugin *sampleClientPlugin = (SampleClientPlugin *)instance;
    [sampleClientPlugin setLocalNotification:notificationId title:title msg:msg interval:interval];
}

void _SampleClientPlugin_CancelLocalNotification(void *instance, int notificationId)
{
    SampleClientPlugin *sampleClientPlugin = (SampleClientPlugin *)instance;
    [sampleClientPlugin cancelLocalNotification:notificationId];
}
