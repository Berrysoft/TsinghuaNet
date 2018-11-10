#pragma once

#define DEPENDENCY_PROPERTY(Name, type)                                                          \
public:                                                                                          \
    type Name() { return winrt::unbox_value<type>(GetValue(m_##Name##Property)); }               \
    void Name(type value) { SetValue(m_##Name##Property, winrt::box_value(value)); }             \
    static Windows::UI::Xaml::DependencyProperty Name##Property() { return m_##Name##Property; } \
                                                                                                 \
private:                                                                                         \
    static Windows::UI::Xaml::DependencyProperty m_##Name##Property;

#define DEPENDENCY_PROPERTY_INIT(Name, type, base, rt_base, ...)                                                      \
    Windows::UI::Xaml::DependencyProperty base::m_##Name##Property = Windows::UI::Xaml::DependencyProperty::Register( \
        L#Name,                                                                                                       \
        winrt::xaml_typename<type>(),                                                                                 \
        winrt::xaml_typename<rt_base>(),                                                                              \
        Windows::UI::Xaml::PropertyMetadata(__VA_ARGS__));

#define EVENT_DECL(Name, type)                                                                                               \
public:                                                                                                                      \
    winrt::event_token Name(Windows::Foundation::EventHandler<type> const& handler) { return m_##Name##Event.add(handler); } \
    void Name(winrt::event_token const& token) { m_##Name##Event.remove(token); }                                            \
                                                                                                                             \
private:                                                                                                                     \
    winrt::event<Windows::Foundation::EventHandler<type>> m_##Name##Event;
