#pragma once

namespace linq
{
    namespace impl
    {
        template <typename Collection, typename Iter = decltype(std::declval<Collection>().First())>
        class winrt_container_enumerator
        {
        private:
            Collection m_collection;
            Iter m_iter;

        public:
            constexpr winrt_container_enumerator(Collection const& collection) : m_collection(collection), m_iter(m_collection.First()) {}

            constexpr operator bool() const { return m_iter.HasCurrent(); }
            constexpr winrt_container_enumerator& operator++()
            {
                m_iter.MoveNext();
                return *this;
            }
            constexpr decltype(auto) operator*() { return m_iter.Current(); }
        };

        template <typename Collection, typename = std::enable_if_t<winrt::impl::has_GetAt<Collection>::value>>
        class winrt_fast_container_enumerator
        {
        private:
            Collection m_collection;
            std::uint32_t m_index, m_size;

        public:
            constexpr winrt_fast_container_enumerator(Collection const& collection) : m_collection(collection), m_index(0), m_size(m_collection.Size()) {}

            constexpr operator bool() const { return m_index < m_size; }
            constexpr winrt_fast_container_enumerator& operator++()
            {
                ++m_index;
                return *this;
            }
            constexpr decltype(auto) operator*() { return m_collection.GetAt(m_index); }
        };
    } // namespace impl

    template <typename C, typename = void>
    constexpr bool has_GetAt_v{ false };

    template <typename C>
    constexpr bool has_GetAt_v<C, std::void_t<decltype(std::declval<C>().GetAt(0))>>{ true };

    template <typename Collection, typename Query, typename = decltype(std::declval<Collection>().First())>
    constexpr auto operator>>(Collection const& c, Query&& q)
    {
        if constexpr (has_GetAt_v<Collection>)
        {
            return std::forward<Query>(q)(enumerable(impl::winrt_fast_container_enumerator<Collection>(c)));
        }
        else
        {
            return std::forward<Query>(q)(enumerable(impl::winrt_container_enumerator<Collection>(c)));
        }
    }
} // namespace linq
