#pragma once
#include "UserContentTypeCheckedConverter.g.h"

#include "EnumCheckedConverter.h"

namespace winrt::TsinghuaNetUWP::implementation
{
    struct UserContentTypeCheckedConverter : UserContentTypeCheckedConverterT<UserContentTypeCheckedConverter>, EnumCheckedConverter<UserContentType>
    {
        UserContentTypeCheckedConverter() = default;
    };
} // namespace winrt::TsinghuaNetUWP::implementation

namespace winrt::TsinghuaNetUWP::factory_implementation
{
    struct UserContentTypeCheckedConverter : UserContentTypeCheckedConverterT<UserContentTypeCheckedConverter, implementation::UserContentTypeCheckedConverter>
    {
    };
} // namespace winrt::TsinghuaNetUWP::factory_implementation
