//
//  EMWrapperCallback.m
//  wrapper
//
//  Created by 杜洁鹏 on 2022/11/15.
//

#import "EMWrapperCallback.h"
#import "EMThreadQueue.h"
#import "EMUtil.h"
#import "EMError+Helper.h"

@implementation EMWrapperCallback

- (void)onSuccess:(NSObject *)aObj {
    __weak EMWrapperCallback *weakSelf = self;
    [weakSelf runInQueue:^{
        weakSelf.onSuccessCallback( [(NSDictionary *)aObj toJsonString]);
    }];
}

- (void)onError:(EMError *)aError {
    __weak EMWrapperCallback *weakSelf = self;
    [weakSelf runInQueue:^{
        weakSelf.onErrorCallback([[aError toJson] toJsonString]);
    }];
}

- (void)onProgress:(int)aProgress{
    __weak EMWrapperCallback *weakSelf = self;
    [weakSelf runInQueue:^{
        weakSelf.onProgressCallback(aProgress);
    }];
}

- (void)runInQueue:(void(^)(void))aciton {
    [EMThreadQueue mainQueue:aciton];
}

@end
