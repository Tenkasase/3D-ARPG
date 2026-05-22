using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件信息接口，所有事件信息类都必须实现这个接口
/// </summary>
public interface IEventInfo
{
}

/// <summary>
/// 事件信息类，一个参数
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

/// <summary>
/// 事件信息类，无参数
/// </summary>
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventCenter : BaseManager<EventCenter>
{    
    // 事件字典，key为事件名字，value为事件触发时调用的函数
    private Dictionary<EventName, IEventInfo> eventDic =  new Dictionary<EventName, IEventInfo>();
    
    private EventCenter() { }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="eventName"></param>
    public void EventTrigger<T>(EventName eventName, T info)
    {
        if(eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).actions?.Invoke(info);
    }

    public void EventTrigget(EventName eventName)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).actions?.Invoke();
    }

    /// <summary>
    /// 添加事件监听者
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="func">监听函数</param>
    public void AddEventListener<T>(EventName eventName, UnityAction<T> func)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).actions += func;
        else
            eventDic.Add(eventName, new EventInfo<T>(func));
    }

    public void AddEventListener(EventName eventName, UnityAction func)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).actions += func;
        else
            eventDic.Add(eventName, new EventInfo(func));
    }

    /// <summary>
    /// 移除事件监听者
    /// </summary>
    /// <param name="eventName">事件名字</param>
    /// <param name="func">监听函数</param>
    public void RemoveEventListener<T>(EventName eventName, UnityAction<T> func)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).actions -= func;
    }

    public void RemoveEventListener(EventName eventName, UnityAction func)
    {
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).actions -= func;
    }

    /// <summary>
    /// 清除所有事件的监听者（主要用在切换场景时）
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }

    /// <summary>
    /// 清除 某一个 事件的监听者
    /// </summary>
    /// <param name="eventName"></param>
    public void Clear(EventName  eventName)
    {
        if(eventDic.ContainsKey(eventName))
            eventDic.Remove(eventName);
    }
}
