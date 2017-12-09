using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MvvmValidation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using Type = NokProjectX.Wpf.Entities.Type;

namespace NokProjectX.Wpf.ViewModel.Inventory
{
    public class AddProductViewModel : ValidatableViewModelBase
    {
        private readonly YumiContext _context;

        public AddProductViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();
            Types = _context.Types.ToList();
            ConfigureValidationRules();

        }

        public RelayCommand CloseCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand ViewCommand { get; set; }
        public RelayCommand UploadCommand { get; set; }
        private void LoadCommands()
        {
            ViewCommand = new RelayCommand(OnView);
            AddCommand = new RelayCommand(OnAdd);
            CloseCommand = new RelayCommand(OnClose);
            UploadCommand = new RelayCommand(OnUpload);
        }

        private void OnUpload()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (JPG,PNG)|*.JPG;*.PNG";
            openFileDialog.ShowDialog();
            if (!openFileDialog.CheckFileExists)
            {
                MessageBox.Show("File not exist");
                return;
            }
       
            if (Path.GetExtension(openFileDialog.FileName) == ".jpg" || Path.GetExtension(openFileDialog.FileName) == ".png")
            {
                MessageBox.Show((openFileDialog.FileName));

                Picture = ImageToByteArray(new Bitmap(openFileDialog.FileName));
            }
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            var returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        private void OnClose()
        {
            
            DialogHost.CloseDialogCommand.Execute(this, null);
            Clear();
            IsOpen = false;
            Validator.Reset();

        }

        private async void OnAdd()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }
            var result =_context.Products.Select(c => c.ProductCode).OrderByDescending(c => c).FirstOrDefault();
            int newCode = result + 1;
            //var picture = ImageToByteArray(Picture); 
            Product newProduct = new Product()
            {
                
                ProductCode = newCode,
                Name = ProductName,
                Description = Description,
                Type = SelectedType,
                Stock = Int32.Parse(Stock),
                Price = Double.Parse(Price),
                Image = Picture
            };
            _context.Products.Add(newProduct);
            _context.SaveChanges();
            MessengerInstance.Send(new RefreshMessage());
            
            OnClose();
        }

        void Clear()
        {
            ProductName = null;
            Description = null;
            SelectedType = null;
            Stock = null;
            Price = null;
        }

        private void OnView()
        {
            if (IsOpen)
            {
                IsOpen = false;
                return;
            }
            IsOpen = true;
        }

        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }

        private byte _image;
        public byte Image { get { return _image; } set { Set(ref _image, value); } }

        private string _productName;
        public string ProductName { get { return _productName; } set { Set(ref _productName, value); } }
        private string _description;
        public string Description { get { return _description; } set { Set(ref _description, value); } }
        private string _type;
        public string Type { get { return _type; } set { Set(ref _type, value); } }
        private string _stock;
        public string Stock { get { return _stock; } set { Set(ref _stock, value); } }
        private string _price;
        public string Price { get { return _price; } set { Set(ref _price, value); } }
        private byte[] _picture;
        public byte[] Picture { get { return _picture; } set { Set(ref _picture, value); } }

        private List<Type> _types;
        public List<Type> Types { get { return _types; }set { Set(ref _types,value); } }
        private Type _selectedType;
        public Type SelectedType { get { return _selectedType; } set { Set(ref _selectedType, value); } }


        private async void Validate()
        {
            await ValidateAsync();
        }

        public async Task ValidateAsync()
        {
            await Validator.ValidateAllAsync();
            
        }

        private void ConfigureValidationRules()
        {


            //            Validator.AddAsyncRule(nameof(LRN),
            //                async () =>
            //                {
            //                    var _context = new MorenoContext();
            //                    var result = await _context.Students.FirstOrDefaultAsync(e => e.LRN == LRN);
            //                    bool isAvailable = result == null;
            //                    return RuleResult.Assert(isAvailable,
            //                        string.Format("LRN {0} is taken. Please choose a different one.", LRN));
            //                });
            Validator.AddRequiredRule(() => ProductName, "Product Name is required");
            Validator.AddRule(nameof(ProductName),
                 () =>
                {
                    var count = _context.Products.Count(c => c.Name.ToLower().Equals(ProductName.Trim().ToLower()));
                    var result = count == 0;
                    return RuleResult.Assert(result,
                        $"Product already exists");
                });

            Validator.AddRequiredRule(() => Description, "Description is required");

            Validator.AddRequiredRule(() => SelectedType, "Type is required");

            Validator.AddRequiredRule(() => Stock, "Stock is required");

            Validator.AddRule(nameof(Stock),
                () =>
                {
                    int num;
                    var result = int.TryParse(Stock, out num);
                    return RuleResult.Assert(result,
                        $"Stock must be a number.");
                });

            Validator.AddRequiredRule(() => Price, "Price is required");

            Validator.AddRule(nameof(Price),
                () =>
                {
                    double num;
                    var result = double.TryParse(Price, out num);
                    return RuleResult.Assert(result,
                        $"Price must be a number.");
                });


        }

    }
}