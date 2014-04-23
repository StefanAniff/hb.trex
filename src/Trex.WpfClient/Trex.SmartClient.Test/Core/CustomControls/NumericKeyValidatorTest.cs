using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MS.Internal.Text.TextInterface;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Trex.SmartClient.Core.CustomControls;
using Trex.SmartClient.Test.TestUtil;
using System.Linq;
using ServiceStack.Common.Extensions;

namespace Trex.SmartClient.Test.Core.CustomControls
{
    [TestFixture]
    public class NumericKeyValidatorTest : AutoFixtureTestBase
    {
        [TestCase(Key.A, Key.None, ModifierKeys.None, false)]
        [TestCase(Key.System, Key.F10, ModifierKeys.Shift, true)]
        [TestCase(Key.Escape, Key.None, ModifierKeys.None, true)]
        [TestCase(Key.Tab, Key.None, ModifierKeys.None, true)]
        [TestCase(Key.OemComma, Key.None, ModifierKeys.None, true)]
        [TestCase(Key.D1, Key.None, ModifierKeys.None, true)]
        [TestCase(Key.OemMinus, Key.None, ModifierKeys.None, false)]
        public void OnUserInput_SingleInput_TestCase(Key key, Key systemKey, ModifierKeys modifierKeys, bool expectedResult)
        {
            // Arrange
            var fixture = InitializeFixture();

            var sut = fixture.Create<NumericTextBox.NumericKeyValidator>();

            // Act
            var result = sut.IsValid(key, systemKey, modifierKeys);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(@"^[a-zA-Z]+$", Key.None, 1, ModifierKeys.None, false)]
        [TestCase(@"^[D0-D9_]+$", Key.None, 2, ModifierKeys.None, true)]
        [TestCase(@"^[F1-F12_]+$", Key.None, 2, ModifierKeys.None, true)]
        public void OnUserInput_RangeInput_TestCase(string regExpression, Key systemKey, int keyStringLength, ModifierKeys modifierKeys, bool expectedResult)
        {
            // Arrange
            var fixture = InitializeFixture();

            var keys = Enum.GetValues(typeof(Key)).ToList<Key>();
            var stringKeysPaired = keys.Select(x => new KeyValuePair<string, Key>(x.ToString(), x));

            var assertionWasMade = false;
            
            var sut = fixture.Create<NumericTextBox.NumericKeyValidator>();

            // Act
            // Assert
            foreach (var keyString in stringKeysPaired)
            {
                if (keyString.Key.Length == keyStringLength && Regex.IsMatch(keyString.Key, regExpression))
                {
                    var result = sut.IsValid(keyString.Value, systemKey, modifierKeys);
                    Assert.That(result, Is.EqualTo(expectedResult));
                    assertionWasMade = true;
                }
            }            

            Assert.That(assertionWasMade, Is.True);
        }
    }
}