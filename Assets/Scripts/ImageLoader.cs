using Cysharp.Threading.Tasks;
using SimpleFileBrowser;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    private Texture2D _tempTexture;
    private string _savedImagePath;
    private string _savedImageName;

    private void Start()
    {
        FileBrowser.SetFilters(true, new[] { new FileBrowser.Filter("Images", ".jpg", ".jpeg", ".png") });
        FileBrowser.SetDefaultFilter(".png");
        FileBrowser.AddQuickLink("Pictures", "/storage/emulated/0/Pictures");
    }

    public void LoadImage()
    {
#if UNITY_ANDROID
        LoadImageFromMobile();
#else
        StartCoroutine(ShowLoadDialogCoroutine());
#endif
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog
            (FileBrowser.PickMode.Files, 
            false, 
            "Выберите изображение", 
            "Загрузить");

        if (FileBrowser.Success)
        {
            string filePath = FileBrowser.Result[0];
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);

            if (tex.LoadImage(fileData))
            {
                _tempTexture = CropImageToSquare(tex);
                targetImage.sprite = Sprite.Create(_tempTexture, 
                    new Rect(0, 0, _tempTexture.width, _tempTexture.height), 
                    new Vector2(0.5f, 0.5f));
            }
        }
    }

    private void LoadImageFromMobile()
    {
        NativeGallery.Permission permission = NativeGallery.CheckPermission
            (NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

        if (permission == NativeGallery.Permission.Granted)
        {
            NativeGallery.GetImageFromGallery((path) =>
            {
                if (path != null)
                {
                    Texture2D tex = NativeGallery.LoadImageAtPath(path);

                    if (tex != null)
                    {
                        _tempTexture = CropImageToSquare(tex); 
                        targetImage.sprite = Sprite.Create
                        (_tempTexture, 
                        new Rect(0, 0, _tempTexture.width, _tempTexture.height), 
                        new Vector2(0.5f, 0.5f));
                    }
                }
            }, "Select an image");
        }
    }

    private Texture2D CropImageToSquare(Texture2D originalTexture)
    {
        int size = Mathf.Min(originalTexture.width, originalTexture.height);
        Texture2D croppedTexture = new Texture2D(size, size);

        int startX = (originalTexture.width - size) / 2;
        int startY = (originalTexture.height - size) / 2;

        Color[] pixels = originalTexture.GetPixels(startX, startY, size, size);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        return croppedTexture;
    }

    public async UniTask SaveImageAsync()
    {
        if (_tempTexture != null)
        {
            string saveDirectory = Path.Combine(Application.dataPath, "SaveData/Images");
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            _savedImageName = $"Image_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
            _savedImagePath = Path.Combine(saveDirectory, _savedImageName);

            byte[] imageBytes = _tempTexture.EncodeToPNG();

            await File.WriteAllBytesAsync(_savedImagePath, imageBytes);
        }
        else
        {
            Debug.LogWarning("Нет временного изображения для сохранения!");
        }
    }

    public void DeleteSavedImage(string addressSprite)
    {
        if (!string.IsNullOrEmpty(addressSprite) && File.Exists(addressSprite))
        {
            File.Delete(addressSprite);
            Debug.Log($"Изображение удалено по пути: {addressSprite}");

            _savedImagePath = null;
            _savedImageName = null;
        }
        else
        {
            Debug.LogWarning("Файл для удаления не найден или не был сохранен!");
        }
    }

    public string GetSavedImagePath()
    {
        return _savedImagePath;
    }

    public string GetSavedImageName()
    {
        return _savedImageName;
    }
}
