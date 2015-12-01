//
//  SampleAppController.m
//  Unity-iPhone
//
//  Created by kitahara on 12/2/15.
//
//

// -*- mode:objc -*-
#import "SampleAppController.h"

void _ios8OverRegisterLocalNotificaton()
{
#if defined(__IPHONE_8_0)
    if (_ios80orNewer) {
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:UIUserNotificationTypeAlert |
                                                UIUserNotificationTypeBadge |
                                                UIUserNotificationTypeSound categories:nil];
        [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
    }
#endif
}

void UnitySendMessage(const char* objectName, const char* methodName, const char* message);

@implementation SampleAppController

@end

IMPL_APP_CONTROLLER_SUBCLASS(SampleAppController)