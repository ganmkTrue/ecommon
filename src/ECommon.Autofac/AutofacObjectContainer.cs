﻿using System;
using Autofac;
using ECommon.Components;

namespace ECommon.Autofac
{
    /// <summary>Autofac implementation of IObjectContainer.
    /// </summary>
    public class AutofacObjectContainer : IObjectContainer
    {
        /// <summary>Represents the iner autofac container builder.
        /// </summary>
        public ContainerBuilder ContainerBuilder { get; }
        /// <summary>Represents the inner autofac container.
        /// </summary>
        public IContainer Container { get; set; }

        /// <summary>Default constructor.
        /// </summary>
        public AutofacObjectContainer() : this(new ContainerBuilder())
        {
        }
        /// <summary>Parameterized constructor.
        /// </summary>
        public AutofacObjectContainer(ContainerBuilder containerBuilder)
        {
            ContainerBuilder = containerBuilder;
        }
        /// <summary>Parameterized constructor.
        /// </summary>
        public AutofacObjectContainer(IContainer container)
        {
            Container = container;
        }

        /// <summary>Build the container.
        /// </summary>
        public void Build()
        {
            Container = ContainerBuilder.Build();
        }
        /// <summary>Register a implementation type.
        /// </summary>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="life">The life cycle of the implementer type.</param>
        public void RegisterType(Type implementationType, string serviceName = null, LifeStyle life = LifeStyle.Singleton)
        {
            if (implementationType.IsGenericType)
            {
                var registrationBuilder = ContainerBuilder.RegisterGeneric(implementationType);
                if (serviceName != null)
                {
                    registrationBuilder.Named(serviceName, implementationType);
                }
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
            else
            {
                var registrationBuilder = ContainerBuilder.RegisterType(implementationType);
                if (serviceName != null)
                {
                    registrationBuilder.Named(serviceName, implementationType);
                }
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
        }
        /// <summary>Register a implementer type as a service implementation.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="serviceName">The service name.</param>
        /// <param name="life">The life cycle of the implementer type.</param>
        public void RegisterType(Type serviceType, Type implementationType, string serviceName = null, LifeStyle life = LifeStyle.Singleton)
        {
            if (implementationType.IsGenericType)
            {
                var registrationBuilder = ContainerBuilder.RegisterGeneric(implementationType).As(serviceType);
                if (serviceName != null)
                {
                    registrationBuilder.Named(serviceName, implementationType);
                }
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
            else
            {
                var registrationBuilder = ContainerBuilder.RegisterType(implementationType).As(serviceType);
                if (serviceName != null)
                {
                    registrationBuilder.Named(serviceName, serviceType);
                }
                if (life == LifeStyle.Singleton)
                {
                    registrationBuilder.SingleInstance();
                }
            }
        }
        /// <summary>Register a implementer type as a service implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementer">The implementer type.</typeparam>
        /// <param name="serviceName">The service name.</param>
        /// <param name="life">The life cycle of the implementer type.</param>
        public void Register<TService, TImplementer>(string serviceName = null, LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            var registrationBuilder = ContainerBuilder.RegisterType<TImplementer>().As<TService>();
            if (serviceName != null)
            {
                registrationBuilder.Named<TService>(serviceName);
            }
            if (life == LifeStyle.Singleton)
            {
                registrationBuilder.SingleInstance();
            }
        }
        /// <summary>Register a implementer type instance as a service implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementer">The implementer type.</typeparam>
        /// <param name="instance">The implementer type instance.</param>
        /// <param name="serviceName">The service name.</param>
        public void RegisterInstance<TService, TImplementer>(TImplementer instance, string serviceName = null)
            where TService : class
            where TImplementer : class, TService
        {
            var registrationBuilder = ContainerBuilder.RegisterInstance(instance).As<TService>().SingleInstance();
            if (serviceName != null)
            {
                registrationBuilder.Named<TService>(serviceName);
            }
        }
        /// <summary>Resolve a service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>The component instance that provides the service.</returns>
        public TService Resolve<TService>() where TService : class
        {
            return Container.Resolve<TService>();
        }
        /// <summary>Resolve a service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>The component instance that provides the service.</returns>
        public object Resolve(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }
        /// <summary>Try to retrieve a service from the container.
        /// </summary>
        /// <typeparam name="TService">The service type to resolve.</typeparam>
        /// <param name="instance">The resulting component instance providing the service, or default(TService).</param>
        /// <returns>True if a component providing the service is available.</returns>
        public bool TryResolve<TService>(out TService instance) where TService : class
        {
            return Container.TryResolve(out instance);
        }
        /// <summary>Try to retrieve a service from the container.
        /// </summary>
        /// <param name="serviceType">The service type to resolve.</param>
        /// <param name="instance">The resulting component instance providing the service, or null.</param>
        /// <returns>True if a component providing the service is available.</returns>
        public bool TryResolve(Type serviceType, out object instance)
        {
            return Container.TryResolve(serviceType, out instance);
        }
        /// <summary>Resolve a service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="serviceName">The service name.</param>
        /// <returns>The component instance that provides the service.</returns>
        public TService ResolveNamed<TService>(string serviceName) where TService : class
        {
            return Container.ResolveNamed<TService>(serviceName);
        }
        /// <summary>Resolve a service.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <param name="serviceType">The service type.</param>
        /// <returns>The component instance that provides the service.</returns>
        public object ResolveNamed(string serviceName, Type serviceType)
        {
            return Container.ResolveNamed(serviceName, serviceType);
        }
        /// <summary>Try to retrieve a service from the container.
        /// </summary>
        /// <param name="serviceName">The name of the service to resolve.</param>
        /// <param name="serviceType">The type of the service to resolve.</param>
        /// <param name="instance">The resulting component instance providing the service, or null.</param>
        /// <returns>True if a component providing the service is available.</returns>
        public bool TryResolveNamed(string serviceName, Type serviceType, out object instance)
        {
            return Container.TryResolveNamed(serviceName, serviceType, out instance);
        }
    }
}

