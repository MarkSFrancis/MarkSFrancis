﻿using MarkSFrancis.Data.LazyLoad;
using MarkSFrancis.Data.Tests.LazyLoad.TestData;
using NUnit.Framework;
using System;

namespace MarkSFrancis.Data.Tests.LazyLoad
{
    public class LazyDatabaseTests
    {
        [Test]
        public void AddingTable_WithNullLoader_ThrowsArgumentNullException()
        {
            var database = new LazyDatabase();

            Assert.Throws<ArgumentNullException>(() => database.TryAddTable<int, Person>(null));
        }

        [Test]
        public void SubmittingEntry_ToTheCache_IncreasesCacheCountByOne()
        {
            var database = new LazyDatabase();
            var people = new PersonRepository();
            var person = people.GetSingle(1);

            database.TryAddTable<int, Person>(people.GetSingle);
            database.TryAddOrUpdate(1, person);

            Assert.AreEqual(1, database.TableItemsCachedCount<Person>());
            Assert.AreEqual(0, database.TableItemsCachedCount<Role>());
        }

        [Test]
        public void SubmittingCrossTypeEntries_ToTheCache_IncreasesCacheCountByTwo()
        {
            var database = new LazyDatabase();
            var people = new PersonRepository();
            var roles = new RoleRepository();
            var person = people.GetSingle(1);
            var role = roles.GetSingle(1);

            database.AddOrUpdate(1, person, people.GetSingle);
            database.AddOrUpdate(1, role, roles.GetSingle);

            Assert.AreEqual(1, database.TableItemsCachedCount<Person>());
            Assert.AreEqual(1, database.TableItemsCachedCount<Role>());
        }

        [Test]
        public void GetFromDatabaseWith1Table_Twice_LoadsOnceFromDatabaseOnceFromCache()
        {
            var database = new LazyDatabase();
            var people = new PersonRepository();

            database.Get(1, people.GetSingle);
            database.Get(1, people.GetSingle);

            Assert.AreEqual(1, people.TimesLoaded);
        }

        [Test]
        public void GetFromDatabaseWithMultipleTablesAndKeys_WithMultipleRequests_LoadsOnceFromDatabasePerKey()
        {
            var database = new LazyDatabase();
            var people = new PersonRepository();
            var roles = new RoleRepository();

            database.TryAddTable<int, Person>(people.GetSingle);
            database.TryAddTable<int, Role>(roles.GetSingle);

            database.TryGet(1, out Person p);
            database.TryGet(1, out p);
            database.TryGet(2, out p);
            database.TryGet(2, out p);

            database.TryGet(1, out Role r);
            database.TryGet(2, out r);
            database.TryGet(7, out r);
            database.TryGet(7, out r);
            database.TryGet(7, out r);
            database.TryGet(7, out r);
            database.TryGet(7, out r);
            database.TryGet(7, out r);
            database.TryGet(7, out r);

            Assert.AreEqual(2, people.TimesLoaded);
            Assert.AreEqual(3, roles.TimesLoaded);
        }

        [Test]
        public void GetFromDatabase_WhenLifetimeHasExpired_Reloads()
        {
            var database = new LazyDatabase(TimeSpan.Zero);
            var people = new PersonRepository();

            database.TryAddTable<int, Person>(people.GetSingle);

            database.TryGet(1, out Person p);
            database.TryGet(1, out p);
            database.Get(1, people.GetSingle);

            Assert.AreEqual(3, people.TimesLoaded);
        }

        [Test]
        public void GetFromDatabase_WhenLifetimeHasNotExpired_DoesNotReload()
        {
            var database = new LazyDatabase(TimeSpan.FromHours(100));
            var people = new PersonRepository();

            database.Get(1, people.GetSingle);
            database.TryGet(1, out Person p);
            database.TryGet(1, out p);

            Assert.AreEqual(1, people.TimesLoaded);
        }
    }
}
