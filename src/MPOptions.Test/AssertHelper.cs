using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPOptions.Test
{
    public static class AssertHelper
    {
        public static void Throws<TException>(Action blockToExecute) where TException : System.Exception
        {
            Throws<TException>(blockToExecute, null);
        }

        public static void ThrowsNoException(Action blockToExecute)
        {
            try
            {
                blockToExecute();
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception of type " + ex.GetType() + " was thrown but no exception was expected.");
            }
        }

        public static void Throws<TException>(Action blockToExecute, Action<TException> exc) where TException : System.Exception
        {
            try
            {
                blockToExecute();
            }
            catch (Exception ex)
            {
                if (ex is UnitTestAssertException)
                    throw;

                Assert.IsTrue(ex.GetType() == typeof(TException), "Expected exception of type " + typeof(TException) + " but type of " + ex.GetType() + " was thrown instead.");
                if (exc != null)
                    exc(ex as TException);
                return;
            }
            Assert.Fail("Expected exception of type " + typeof(TException) + " but no exception was thrown.");
        }
    }
}
