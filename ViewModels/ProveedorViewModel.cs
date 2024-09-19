using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EjercicioCRUDMvvm.Models;
using SQLite;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EjercicioCRUDMvvm.ViewModels
{
    public partial class ProveedorViewModel : ObservableObject
    {
        private SQLiteAsyncConnection _database;
        public ObservableCollection<Proveedor> Proveedores { get; } = new ObservableCollection<Proveedor>();

        [ObservableProperty]
        private Proveedor _proveedor = new Proveedor();

        public ProveedorViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Proveedores.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Proveedor>().Wait();
            LoadProveedores();
        }

        [RelayCommand]
        private async Task AddProveedor()
        {
            if (string.IsNullOrWhiteSpace(Proveedor.Nombre) ||
                string.IsNullOrWhiteSpace(Proveedor.Direccion) ||
                string.IsNullOrWhiteSpace(Proveedor.Telefono) ||
                string.IsNullOrWhiteSpace(Proveedor.Email) ||
                string.IsNullOrWhiteSpace(Proveedor.Empresa))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Por favor complete todos los campos", "OK");
                return;
            }

            await _database.InsertAsync(Proveedor);
            LoadProveedores();
            Proveedor = new Proveedor();
        }

        [RelayCommand]
        private async Task DeleteProveedor(Proveedor proveedor)
        {
            if (proveedor == null) return;
            await _database.DeleteAsync(proveedor);
            LoadProveedores();
        }

        private async void LoadProveedores()
        {
            var proveedores = await _database.Table<Proveedor>().ToListAsync();
            Proveedores.Clear();
            foreach (var proveedor in proveedores)
            {
                Proveedores.Add(proveedor);
            }
        }
    }
}
