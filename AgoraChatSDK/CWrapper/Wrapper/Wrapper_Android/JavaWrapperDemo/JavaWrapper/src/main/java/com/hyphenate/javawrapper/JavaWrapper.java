package com.hyphenate.javawrapper;

import android.content.Context;

import com.hyphenate.wrapper.EMWrapper;
import com.hyphenate.wrapper.EMWrapperListener;

public class JavaWrapper {
    public static EMWrapper wrapper = null;
    public static Context context;
    public static EMWrapperListener listener = null;
    public static EMWrapper wrapper(int iType, long listener) {
        wrapper = new EMWrapper();
        return wrapper;
    }
}
