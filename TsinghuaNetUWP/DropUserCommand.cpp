#include "pch.h"

#include "DropUserCommand.h"

using namespace std;
using namespace winrt;
using namespace Windows::Foundation;

namespace winrt::TsinghuaNetUWP::implementation
{
    void DropUserCommand::Execute(IInspectable const& parameter)
    {
        hstring addr = unbox_value<hstring>(parameter);
        m_DropUserEvent(*this, addr);
    }
} // namespace winrt::TsinghuaNetUWP::implementation
