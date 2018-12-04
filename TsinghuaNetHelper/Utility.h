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
