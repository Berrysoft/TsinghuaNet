// https://www.xamarinhelp.com/masked-entry-in-xamarin-forms/

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PropertyChanged;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Models
{
    class IPMaskedBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private static readonly Regex IPRegex = new Regex(@"\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){1,4}\b");

        [SuppressPropertyChangedWarnings]
        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;

            var text = entry.Text;

            if (string.IsNullOrWhiteSpace(text)) return;

            Match match;

            while (true)
            {
                match = IPRegex.Match(text);
                if (match.Value != text)
                {
                    text = text.Remove(text.Length - 1);
                }
                else
                {
                    break;
                }
            }

            if (entry.Text != text)
            {
                if (text.Count(c => c == '.') < 3) text += '.';
                entry.Text = text;
            }
        }
    }
}
