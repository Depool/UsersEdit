using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ApplicationRepository.Shared
{
    public static class SharedDBContext<T> where T: DbContext, new()
    {
        private static T context;

        public static T SharedContext()
        {
           if (context == null)
               context = new T();
           return context;
        }
    }
}
