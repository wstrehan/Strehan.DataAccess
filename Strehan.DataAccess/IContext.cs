using System;

namespace Strehan.DataAccess
{
    public interface IContext
    {
        object GetMappingItem<T>(object context);
        object GetMappingItem(object context);
    }
}
