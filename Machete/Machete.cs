using System;
using System.Collections.Generic;
using ImpromptuInterface;
using Machete.Contracts;

namespace Machete
{
    public static class Machete
    {
        public static IDisposable Scope
        {
            get { return new MacheteContext(); }
        }

        public static IDisposable CreateScope(Func<dynamic> serviceCreator)
        {
            return new MacheteContext(serviceCreator);
        }

        public static IDisposable Current
        {
            get { return MacheteContext.CurrentContext; }
        }

        public static T CreateService<T>() where T : class
        {
            var currentContext = MacheteContext.CurrentContext;

            if (currentContext == null)
                throw new NotSupportedException("Machete must have an active scope to work in.");

            var bundler = currentContext.GetOrCreateBundler<T>();
            var impromptuBundler = bundler.ActLike<T>();

            return impromptuBundler;
        }

        public static IEnumerable<Response> Slash()
        {
            var currentContext = MacheteContext.CurrentContext;

            if (currentContext == null)
                throw new NotSupportedException("Machete must have an active scope to work in.");

            return currentContext.Slash();
        }
    }
}