using Application.Abstractions.Messaging;
using Domain.Elevators;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Elevator).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ICommand).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
}

