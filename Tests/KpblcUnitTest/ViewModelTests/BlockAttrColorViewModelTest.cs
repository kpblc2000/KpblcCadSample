using KpblcCadCore.ViewModels;
using NUnit.Framework;

namespace KpblcUnitTest.ViewModelTests
{
    public class BlockAttrColorViewModelTest
    {
        [Test]
        public void AttributeTextNotEmpty()
        {
            BlockAttributeColoringViewModel viewModel = new BlockAttributeColoringViewModel();
            viewModel.AttributeText = "foo";
            Assert.IsTrue(viewModel.CanColorAttributeToRed);
            Assert.IsTrue(viewModel.CanColorBlockToBlue);
            Assert.IsTrue(viewModel.OkButtonAvailable);
        }

        [Test]
        public void AttributeTextEmpty()
        {
            BlockAttributeColoringViewModel viewModel = new BlockAttributeColoringViewModel();
            viewModel.AttributeText = "";
            Assert.IsFalse(viewModel.CanColorAttributeToRed);
            Assert.IsFalse(viewModel.CanColorBlockToBlue);
            Assert.IsFalse(viewModel.OkButtonAvailable);
        }
    }
}
