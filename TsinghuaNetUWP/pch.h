#pragma once

#define NOMINMAX
#include <unknwn.h>

#include <hstring.h>

#include "winrt/coroutine.h"

#include "winrt/Windows.ApplicationModel.Activation.h"
#include "winrt/Windows.Foundation.Collections.h"
#include "winrt/Windows.Foundation.h"
#include "winrt/Windows.Networking.Connectivity.h"
#include "winrt/Windows.Storage.h"
#include "winrt/Windows.UI.Xaml.Controls.Primitives.h"
#include "winrt/Windows.UI.Xaml.Controls.h"
#include "winrt/Windows.UI.Xaml.Data.h"
#include "winrt/Windows.UI.Xaml.Interop.h"
#include "winrt/Windows.UI.Xaml.Markup.h"
#include "winrt/Windows.UI.Xaml.Media.Animation.h"
#include "winrt/Windows.UI.Xaml.Media.h"
#include "winrt/Windows.UI.Xaml.Navigation.h"
#include "winrt/Windows.UI.Xaml.Shapes.h"
#include "winrt/Windows.UI.Xaml.h"

#include "winrt/Microsoft.UI.Xaml.Automation.Peers.h"
#include "winrt/Microsoft.UI.Xaml.Controls.Primitives.h"
#include "winrt/Microsoft.UI.Xaml.Media.h"
#include "winrt/Microsoft.UI.Xaml.XamlTypeInfo.h"

#define LINQ_USE_WINRT
#include <linq/winrt.hpp>

#include <linq/aggregate.hpp>
#include <linq/query.hpp>
#include <pplawait.h>
#include <sf/sformat.hpp>
