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

    protected void LoadCsv() //데이터 세팅
    {
        if (TsvAsset == null)
        {
            Debug.Log($"No Csv found");
            return;
        }
        
        bRecordsArray = null;
        
        var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",           // 구분자를 tab으로
            HasHeaderRecord = true,     // 첫 줄이 Header이다.
            Mode = CsvMode.NoEscape,    // 이스케이프 문자가 없음 ( ", @, $ ...이 있어도 효과를 무시하고 텍스트로 인식 )
            BadDataFound = null,        // 잘못된 데이터를 무시
            MissingFieldFound = null    // 에러 방지용
        };

        //using은 Dispose()를 명시적으로 호출한다. Dispose()는 내부적으로Close()를 실행하며
        //이는 파일을 다 읽은 후 자동으로 파일 스트림을 닫아주는 안전한 방식이다.
        //또한 가비지 컬렉터를 기다리지 않고 바로 정리하기 때문에 더 빠르다.
        using (var reader = new StringReader(TsvAsset.text))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter()); //int배열 파싱
            
            var records = csv.GetRecords<T>(); //CsvHelper가 자동으로 헤더명과 클래스의 속성명을 연결해 객체 리스트를 생성
            bRecordsArray = records.ToArray();
        }

        MyDebug.Log(bRecordsArray.Length > 0
            ? $"Success Read {typeof(T).Name}s ::: {TsvAsset.name}"
            : $"No records found in {TsvAsset.name}");
    }

    public T[] GetDataAll() // 모든 데이터 추출
    {
        if (bRecordsArray != null) return bRecordsArray;
        MyDebug.Log($"No records found in GetData {TsvAsset.name}");
        return null;
    }
    
    public T GetData(string key) //키로 데이터 추출
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
