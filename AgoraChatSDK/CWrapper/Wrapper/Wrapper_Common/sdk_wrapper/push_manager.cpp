#include "sdk_wrapper.h"
#include "sdk_wrapper_internal.h"

namespace sdk_wrapper {
	SDK_WRAPPER_API const char* SDK_WRAPPER_CALL PushManager_NotImplement(const char* jstr, const char* cbid = nullptr, char* buf = nullptr)
	{
		return nullptr; // do nothing, since windows or mac platform would not support any push action
	}
}

