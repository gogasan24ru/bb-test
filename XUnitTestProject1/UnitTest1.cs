using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ClassLibrary1;
using Xunit;

namespace XUnitTestProject1
{
    public class UserTest : User
    {
        public UserTest():base()
        { }

        public UserTest Clone()
        {
            return new UserTest()
            {
                Age=this.Age,
                Login = this.Login,
                Passport = new Passport(this.Passport.Serial,this.Passport.Number),
                Name = this.Name,
                Password = this.Password,
                Sex = this.Sex,
                Surname = this.Surname
            };
        }
    }
    public class UserModelTest
    {
        private UserTest valid = new UserTest()
        {
            Login = "Hack1r",
            Passport = new Passport() {Serial = 1111,Number = 222222 },
            Age = 20,
            Name = "»‚‡Ì",
            Surname = "œÛÔÍËÌ",
            Password = "pupk1n",
            Sex = sex.Male,
            
        };
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [Fact(DisplayName = "Correct properties behavior")]
        public void ValidUser()
        {
            Assert.True(ValidateModel(valid).Count==0);
        }

        [Fact(DisplayName = "Age validation test")]
        public void AgeValidation1()
        {
            var local = valid.Clone();
            local.Age = 10;
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Age"));
        }

        #region Passport validation tests
        [Fact(DisplayName = "Passport serial range test 1")]
        public void PassportValidation1()
        {
            var local = new Passport(10000, 0);
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Serial"));
        }

        [Fact(DisplayName = "Passport serial range test 2")]
        public void PassportValidation2()
        {
            var local = new Passport(10000, 0);
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Serial"));
        }

        [Fact(DisplayName = "Passport Number range test 3")]
        public void PassportValidation3()
        {
            var local = new Passport(0, -1);
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Number"));
        }

        [Fact(DisplayName = "Passport Number range test 4")]
        public void PassportValidation4()
        {
            var local = new Passport(0, 1000000);
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Number"));
        } 
        #endregion

        #region Password checks

        [Fact(DisplayName = "Empty password test")]
        public void PasswordValidation1()
        {
            var local = valid.Clone();
            local.Password = "";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Password"));
        }
        [Fact(DisplayName = "Big password test")]
        public void PasswordValidation3()
        {
            var local = valid.Clone();
            local.Password = "123456789";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Password"));
        }
        [Fact(DisplayName = "Password wo digits")]
        public void PasswordValidation4()
        {
            var local = valid.Clone();
            local.Password = "password";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Password"));
        }

        [Fact(DisplayName = "Password cyrillic test")]
        public void PasswordValidation2()
        {
            var local = valid.Clone();
            local.Password = "ﬂ·ÎÓÍÓ1";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Password"));
        }

        [Fact(DisplayName = "Password digit position test 0")]
        public void PasswordValidationD1()
        {
            var local = valid.Clone();
            local.Password = "aaa1";
            var vm = ValidateModel(local);
            Assert.True(vm.Count == 0);
        }
        [Fact(DisplayName = "Password digit position test 1")]
        public void PasswordValidationD2()
        {
            var local = valid.Clone();
            local.Password = "aa1a";
            var vm = ValidateModel(local);
            Assert.True(vm.Count == 0);
        }
        [Fact(DisplayName = "Password digit position test 2")]
        public void PasswordValdationD3()
        {
            var local = valid.Clone();
            local.Password = "a1aa";
            var vm = ValidateModel(local);
            Assert.True(vm.Count == 0);
        }
        [Fact(DisplayName = "Password digit position test 3")]
        public void PasswordValdationD4()
        {
            var local = valid.Clone();
            local.Password = "1aaa";
            var vm = ValidateModel(local);
            Assert.True(vm.Count == 0);
        }

        #endregion
        
        #region Login checks

        [Fact(DisplayName = "Empty Login test")]
        public void LoginValdation1()
        {
            var local = valid.Clone();
            local.Login = "";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Login"));
        }
        [Fact(DisplayName = "Big login test")]
        public void LoginValdation3()
        {
            var local = valid.Clone();
            local.Login = "123456789";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Login"));
        }
        [Fact(DisplayName = "Login cyrylic test")]
        public void LoginValdation2()
        {
            var local = valid.Clone();
            local.Login = "ﬂ·ÎÓÍÓ";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Login"));
        }
        #endregion

        #region Name checks

        [Fact(DisplayName = "Empty Name test")]
        public void NameValdation1()
        {
            var local = valid.Clone();
            local.Name = "";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Name"));
        }
        [Fact(DisplayName = "Big Name test")]
        public void NameValdation3()
        {
            var local = valid.Clone();
            local.Name = "123456789";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Name"));
        }
        [Fact(DisplayName = "Name latin test")]
        public void NameValdation2()
        {
            var local = valid.Clone();
            local.Name = "Imma»Ãﬂ";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Name"));
        }
        [Fact(DisplayName = "Name digit test")]
        public void NameValdation4()
        {
            var local = valid.Clone();
            local.Name = "¬‡Òˇ1";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Name"));
        }
        #endregion

        #region Surname checks

        [Fact(DisplayName = "Empty Surname test")]
        public void SurnameValdation1()
        {
            var local = valid.Clone();
            local.Surname = "";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Surname"));
        }
        [Fact(DisplayName = "Big Surname test")]
        public void SurnameValdation3()
        {
            var local = valid.Clone();
            local.Surname = "123456789";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Surname"));
        }
        [Fact(DisplayName = "Surname latin test")]
        public void SurnameValdation2()
        {
            var local = valid.Clone();
            local.Surname = "Imma»Ãﬂ";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Surname"));
        }
        [Fact(DisplayName = "Surname digit test")]
        public void SurnameValdation4()
        {
            var local = valid.Clone();
            local.Surname = "¬‡Òˇ1";
            var vm = ValidateModel(local);
            Assert.True(vm[0].MemberNames.Contains("Surname"));
        }
        #endregion
    }
}
