﻿#pragma once

#include "winrt/Windows.Data.Json.h"
#include "winrt/Windows.Foundation.Collections.h"
#include "winrt/Windows.Foundation.h"
#include "winrt/Windows.Storage.Streams.h"
#include "winrt/Windows.UI.Xaml.Interop.h"
#include "winrt/Windows.Web.Http.h"
#include "winrt/coroutine.h"

#define LINQ_USE_WINRT
#include <linq/winrt.hpp>

#include <linq/query.hpp>
#include <pplawait.h>
#include <sf/sformat.hpp>
