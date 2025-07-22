using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public interface IKeyedData
{
    string Key { get; set; }
}

public abstract class AParsingData<T> : ScriptableObject where T : class, IKeyedData
{
    [SerializeField, ReadOnly] private T[] bRecordsArray = null;
    
    [field: SerializeField]
    public TextAsset TsvAsset { get; private set; }

    protected void OnEnable()
    {
        LoadCsv();
    }

    protected void LoadCsv() //������ ����
    {
        if (TsvAsset == null)
        {
            Debug.Log($"No Csv found");
            return;
        }
        
        bRecordsArray = null;
        
        var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",           // �����ڸ� tab����
            HasHeaderRecord = true,     // ù ���� Header�̴�.
            Mode = CsvMode.NoEscape,    // �̽������� ���ڰ� ���� ( ", @, $ ...�� �־ ȿ���� �����ϰ� �ؽ�Ʈ�� �ν� )
            BadDataFound = null,        // �߸��� �����͸� ����
            MissingFieldFound = null    // ���� ������
        };

        //using�� Dispose()�� ��������� ȣ���Ѵ�. Dispose()�� ����������Close()�� �����ϸ�
        //�̴� ������ �� ���� �� �ڵ����� ���� ��Ʈ���� �ݾ��ִ� ������ ����̴�.
        //���� ������ �÷��͸� ��ٸ��� �ʰ� �ٷ� �����ϱ� ������ �� ������.
        using (var reader = new StringReader(TsvAsset.text))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter()); //int�迭 �Ľ�
            
            var records = csv.GetRecords<T>(); //CsvHelper�� �ڵ����� ������ Ŭ������ �Ӽ����� ������ ��ü ����Ʈ�� ����
            bRecordsArray = records.ToArray();
        }

        MyDebug.Log(bRecordsArray.Length > 0
            ? $"Success Read {typeof(T).Name}s ::: {TsvAsset.name}"
            : $"No records found in {TsvAsset.name}");
    }

    public T[] GetDataAll() // ��� ������ ����
    {
        if (bRecordsArray != null) return bRecordsArray;
        MyDebug.Log($"No records found in GetData {TsvAsset.name}");
        return null;
    }
    
    public T GetData(string key) //Ű�� ������ ����
    {
        foreach (T record in bRecordsArray)
        {
            if (record.Key == key)
                return record;
        }
        
        MyDebug.Log($"No records found in GetData {TsvAsset.name}");
        
        return null;
    }
}
