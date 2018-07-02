/*
 Copyright (C) 2018 William Strehan
*/

using System.Collections.Generic;

namespace Strehan.DataAccess
{
    public interface IDataAccess
    {
        T GetObjectById<TID, T>(TID id, object context = null);
        T GetObjectWithParameters<T>(object inputParameterObj, object context = null);
        List<T> GetList<T>(object context = null);
        List<T> GetListWithParameters<T>(object inputParameterObj, object context = null);
        TID Insert<TID, T>(T insertObj, object context = null);
        void UpdateById<TID, T>(TID id, T updateObj, object context = null);
        void IdCall<TID>(TID id, object context = null);
        void IdCall<TID,T>(TID id, object context = null);
        void ParameterCall(object inputParameterObj, object context);
        void NoParameterCall(object context);
    }
}
