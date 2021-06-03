
using DaprApp.Interface.statestore;
using DataModel.EventBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaprApp.Services
{
    public class DaprStateStoreServices : IStateStoreEvent
    {
        public Task DeleteStateEvent<Target>(Target targetData) where Target : EventDataBase
        {
            throw new NotImplementedException();
        }

        public Task<Target> GetStateEvent<Target>(Target targetData) where Target : EventDataBase
        {
            throw new NotImplementedException();
        }

        public Task SaveStateEvent<Target>(Target targetData) where Target : EventDataBase
        {
            throw new NotImplementedException();
        }

        public Task UpdateStateEvent<Target>(Target targetData) where Target : EventDataBase
        {
            throw new NotImplementedException();
        }
    }
}
