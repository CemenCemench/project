using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Books
{
    public class Core
    {

        public static int VOID = -255;
        public static int NONE = -1;
        //**************************************************************************************************************
        private static Data.LibraryEntities _Databasse;
        public static Data.LibraryEntities Databasse
        {
            get
            {
                if (_Databasse == null)
                {
                    _Databasse = new Data.LibraryEntities();
                }
                return _Databasse;
            }
        }
        //**************************************************************************************************************
        public static MainWindow AppmainWindow;

        public static Data.Users CurrentUsers;

        public static void CancelAllCanges() // для всех записей 
        {
            var ChengedEnries = Databasse.ChangeTracker.Entries().Where(E => E.State != EntityState.Unchanged).ToList();
            foreach(DbEntityEntry E in ChengedEnries)
            {
                CancelCheanges(E);
            }
        }

        public static void CancelCheanges(object Entity) //для одной записи
        {
            DbEntityEntry Entry = Databasse.Entry(Entity);
            try
            {
                switch (Entry.State)
                {
                    case EntityState.Added: Entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        Entry.CurrentValues.SetValues(Entry.OriginalValues);
                        Entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        Entry.State = EntityState.Unchanged;
                        break;
                }
            }
            catch
            {
                Entry.Reload();
            }
        }
        public static Style GetStyle(String StyleName)
        {
            object Style;
            try
            {
                Style = Application.Current.FindResource(StyleName);
            }
            catch
            {
                Style = null;
            }
            if (Style != null && Style is Style)
            {
                return Style as Style;
            }
            else
            {
                MessageBox.Show("Ошибка Стиливого оформления!", "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.None);
                return null;
            }
        }
    }
}
