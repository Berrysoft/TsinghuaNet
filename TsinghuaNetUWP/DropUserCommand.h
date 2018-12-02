#pragma once

#include "DropUserCommand.g.h"

#include "DependencyHelper.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct DropUserCommand : DropUserCommandT<DropUserCommand>
    {
        DropUserCommand() = default;

        bool CanExecute(Windows::Foundation::IInspectable const&) { return true; }
        void Execute(Windows::Foundation::IInspectable const& parameter);

        EVENT_DECL(CanExecuteChanged, Windows::Foundation::IInspectable)
        EVENT_DECL(DropUser, winrt::hstring)
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct DropUserCommand : DropUserCommandT<DropUserCommand, implementation::DropUserCommand>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation
