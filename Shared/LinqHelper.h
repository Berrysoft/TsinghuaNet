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
    } // namespace impl

    template <typename Collection, typename = decltype(std::declval<Collection>().First())>
    constexpr auto winrt_enumerable(Collection const& collection)
    {
        return enumerable(impl::winrt_container_enumerator<Collection>(collection));
    }

    template <typename Collection, typename Query, typename = decltype(std::declval<Collection>().First())>
    constexpr auto operator>>(Collection const& c, Query&& q)
    {
        return std::forward<Query>(q)(winrt_enumerable<Collection>(c));
    }
} // namespace linq
