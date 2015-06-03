//
//  LocalNotification.m
//  NativeToolkit
//
//  Created by Ryan on 29/01/2015.
//
//

#import <Foundation/NSObjCRuntime.h>
#import "LocalNotifications.h"
#import "StringTools.h"

@implementation LocalNotifications

@end

extern "C"
{
    void scheduleNotification(char* title, char* message, int delayInMinutes, char* sound)
    {
        if(floor(NSFoundationVersionNumber) > NSFoundationVersionNumber_iOS_7_1)
        {
            // Register for notifications if iOS 8
            UIUserNotificationType types = UIUserNotificationTypeBadge | UIUserNotificationTypeSound | UIUserNotificationTypeAlert;
            UIUserNotificationSettings *mySettings = [UIUserNotificationSettings settingsForTypes:types categories:nil];
            [[UIApplication sharedApplication] registerUserNotificationSettings:mySettings];
            
            NSLog(@"###### register for notifications");
        }
        
        NSDate* currentDate = [NSDate date];
        NSDate* notifyDate = [currentDate dateByAddingTimeInterval:delayInMinutes*60]; //*60
        NSString *nsSound = [StringTools createNSString:sound];

        UILocalNotification *notification = [[UILocalNotification alloc] init];
        if (notification)
        {
            notification.fireDate = notifyDate;
            notification.timeZone = [NSTimeZone defaultTimeZone];
            notification.applicationIconBadgeNumber = [[UIApplication sharedApplication] applicationIconBadgeNumber] + 1;
            notification.repeatInterval = 0;
            notification.alertBody = [StringTools createNSString:message];
            
            if([nsSound isEqualToString:@"default_sound"])
                notification.soundName = UILocalNotificationDefaultSoundName;
            else if(![nsSound isEqualToString:@""])
                notification.soundName = [nsSound stringByAppendingString:@".caf"];
            
            [[UIApplication sharedApplication] scheduleLocalNotification:notification];
            
             NSLog(@"###### send notification");
        }
    }
    
    void clearNotifications()
    {
        NSLog(@"###### clear notifications");
        
        [[UIApplication sharedApplication] setApplicationIconBadgeNumber: 0];
        [[UIApplication sharedApplication] cancelAllLocalNotifications];
    }
}