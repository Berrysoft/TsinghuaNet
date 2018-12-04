#pragma once

#define PROP_DECL(Name, type)                   \
public:                                         \
    type Name() { return m_##Name; }            \
    void Name(type value) { m_##Name = value; } \
                                                \
private:                                        \
    type m_##Name;

#define PROP_DECL_REF(Name, type)                      \
public:                                                \
    type Name() { return m_##Name; }                   \
    void Name(type const& value) { m_##Name = value; } \
                                                       \
private:                                               \
    type m_##Name;

#define DEPENDENCY_PROPERTY(Name, type)                                                          \
public:                                                                                          \
    type Name() const { return winrt::unbox_value<type>(GetValue(m_##Name##Property)); }         \
    void Name(type value) const { SetValue(m_##Name##Property, winrt::box_value(value)); }       \
    static Windows::UI::Xaml::DependencyProperty Name##Property() { return m_##Name##Property; } \
                                                                                                 \
private:                                                                                         \
    static Windows::UI::Xaml::DependencyProperty m_##Name##Property;

#define DEPENDENCY_PROPERTY_INIT(Name, type, base, ns, ...)          \
    Windows::UI::Xaml::DependencyProperty base::m_##Name##Property = \
        Windows::UI::Xaml::DependencyProperty::Register(             \
            L#Name,                                                  \
            winrt::xaml_typename<type>(),                            \
            winrt::xaml_typename<ns::base>(),                        \
            Windows::UI::Xaml::PropertyMetadata(__VA_ARGS__));

namespace winrt
{
    inline std::wostream& operator<<(std::wostream& os, hstring const& s)
    {
        return os << std::wstring_view(s);
    }
    inline std::ostream& operator<<(std::ostream& os, hstring const& s)
    {
        return os << to_string(s);
    }
} // namespace winrt
