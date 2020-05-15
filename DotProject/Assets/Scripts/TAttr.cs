using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =true)]
public class BaseAttr : Attribute
{

}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class TestAttr1 : BaseAttr
{

}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class TestAttr2 : BaseAttr
{

}


public class TAttr : MonoBehaviour
{
    [TestAttr1]
    [TestAttr2]
    public bool isShow;
}
