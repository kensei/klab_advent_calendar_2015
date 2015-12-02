//
//  SampleAppController.m
//  Unity-iPhone
//
//  Created by kitahara on 12/2/15.
//
//

// -*- mode:objc -*-
#import "UnityAppController.h"

@interface SampleAppController : UnityAppController
+(void)load;
@end

@implementation SampleAppController

+(void)load
{
    extern const char* AppControllerClassName;
    AppControllerClassName = "SampleAppController";
}

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    float version = [[[UIDevice currentDevice] systemVersion] floatValue];
    if (version >= 8.0)
    {
        if ([application respondsToSelector:@selector(registerUserNotificationSettings:)]) {
            UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:UIUserNotificationTypeAlert |
                                                    UIUserNotificationTypeBadge |
                                                    UIUserNotificationTypeSound categories:nil];
            [application registerUserNotificationSettings:settings];
        }
    }
    return YES;
}

@end