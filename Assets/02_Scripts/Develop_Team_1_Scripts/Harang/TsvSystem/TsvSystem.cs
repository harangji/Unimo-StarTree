// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
// using System.Text;
// using UnityEngine;
// using CsvHelper;
// using CsvHelper.Configuration;
//
// // public interface IKeyedData
// // {
// //     string Key { get; }
// // }
//
// public class TsvSystem : SingletonBehaviour<TsvSystem>
// {
//     // public static TsvSystem Instance { get; private set; }
//     //
//     // private void Awake()
//     // {
//     //     Instance = this;
//     // }
//     
//     public void WriteRecordsToTsv<T>(IEnumerable<T> records, string fileName)
//     {
//         if (records == null || !records.Any())
//         {
//             // Debug.LogWarning("No records to write.");
//             return;
//         }
//
//         string path = Path.Combine(Application.streamingAssetsPath, fileName);
//
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//         {
//             Delimiter = "\t",
//             HasHeaderRecord = true,
//             Mode = CsvMode.NoEscape,
//             BadDataFound = null
//         };
//         
//         using (var writer = new StreamWriter(path, false, Encoding.UTF8))
//         using (var csv = new CsvWriter(writer, csvConfig))
//         {
//             csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter());  //int배열 파싱
//             csv.WriteRecords(records);
//         }
//         
//         // Debug.Log($"Successfully wrote {records.Count()} records to TSV at {path}");
//     }
//     
//     /*public void WritePlayersToTsv(FieldPlayer.PlayerInfo[] players)
//     {
//         if (players == null || players.Length == 0)
//         {
//             Debug.LogWarning("No players to write.");
//             return;
//         }
//
//         string path = Path.Combine(Application.streamingAssetsPath, "PlayerTable.tsv");
//
//         using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
//         {
//             // Header
//             writer.WriteLine("Key\tName\tCurrentHp\tMaxHp\thasSkills\tequippedSkills\tPositionX\tPositionY\tPositionZ\tAngleX\tAngleY\tAngleZ");
//
//             // Body
//             foreach (var player in players)
//             {
//                 writer.WriteLine(player.ToTsv());
//             }
//         }
//
//         Debug.Log($"Successfully wrote {players.Length} players to TSV at {path}");
//     }*/
//
//     public T[] ReadRecordsFromTsv<T>(string fileName, string? targetKey = null) where T : IKeyedData //제네릭 타입에 Key속성을 보장
//     {
//         T[] recordsArray = null;
//
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//         {
//             Delimiter = "\t",
//             HasHeaderRecord = true,
//             Mode = CsvMode.NoEscape,
//             BadDataFound = null
//         };
//
//         string path = Path.Combine(Application.streamingAssetsPath, fileName);
//
//         if (!File.Exists(path))
//         {
//             // Debug.LogWarning($"File not found: {path}");
//             return null;
//         }
//
//         using (var reader = new StreamReader(path))
//         using (var csv = new CsvReader(reader, csvConfig))
//         {
//             csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter());
//
//             var records = csv.GetRecords<T>();
//
//             // 키가 있다면 해당 키에 해당하는 레코드만 필터링
//             if (targetKey != null)
//             {
//                 var matched = records.FirstOrDefault(r => r.Key == targetKey);
//                 recordsArray = matched != null ? new T[] { matched } : Array.Empty<T>();
//             }
//             else
//             {
//                 recordsArray = records.ToArray();
//             }
//         }
//
//         MyDebug.Log(recordsArray.Length > 0
//             ? $"Success Read {typeof(T).Name}s ::: {path}"
//             : $"No records found in {path} (Key: {targetKey})");
//
//         return recordsArray;
//     }
//
//     //-------------------------------------------------------------
//     public T[] ReadRecordsFromTsv<T>(string fileName)
//     {
//         T[] recordsArray = null;
//
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
//         {
//             Delimiter = "\t",           //구분자를 tab으로
//             HasHeaderRecord = true,     //첫 줄이 Header이다.
//             Mode = CsvMode.NoEscape,    //이스케이프 문자가 없음 ( ", @, $ ...이 있어도 효과를 무시하고 텍스트로 인식 )
//             BadDataFound = null         //잘못된 데이터를 무시
//         };
//
//         string path = Path.Combine(Application.streamingAssetsPath, fileName);
//
//         if (!File.Exists(path))
//         {
//             // Debug.LogWarning($"File not found: {path}");
//             return null;
//         }
//
//         //using은 Dispose()를 명시적으로 호출한다. Dispose()는 내부적으로Close()를 실행하며
//         //이는 파일을 다 읽은 후 자동으로 파일 스트림을 닫아주는 안전한 방식이다.
//         //또한 가비지 컬렉터를 기다리지 않고 바로 정리하기 때문에 더 빠르다.
//         using (var reader = new StreamReader(path))
//         using (var csv = new CsvReader(reader, csvConfig))
//         {
//             csv.Context.TypeConverterCache.AddConverter<int[]>(new IntArrayConverter()); //int배열 파싱
//             
//             var records = csv.GetRecords<T>(); //CsvHelper가 자동으로 헤더명과 클래스의 속성명을 연결해 객체 리스트를 생성
//             recordsArray = records.ToArray();
//         }
//
//         MyDebug.Log(recordsArray.Length > 0
//             ? $"Success Read {typeof(T).Name}s ::: {path}"
//             : $"No records found in {path}");
//
//         return recordsArray;
//     }
//
//
//     /*private Player[] GetPlayersOrNull()
//     {
//         Player[] players = null;
//         
//         var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) //숫자나 날짜 같은 데이터 형식을 글로벌로 설정
//         {
//             Delimiter = "\t",           //구분자를 tab으로
//             HasHeaderRecord = true,     //첫 줄이 Header이다.
//             Mode = CsvMode.NoEscape,    //이스케이프 문자가 없음 ( ", @, $ ...이 있어도 효과를 무시하고 텍스트로 인식 )
//             BadDataFound = null         //잘못된 데이터를 무시
//         };
//
//         string path = Path.Combine(Application.streamingAssetsPath, "PlayerTable.tsv");
//
//         //using은 Dispose()를 명시적으로 호출한다. Dispose()는 내부적으로Close()를 실행하며
//         //이는 파일을 다 읽은 후 자동으로 파일 스트림을 닫아주는 안전한 방식이다.
//         //또한 가비지 컬렉터를 기다리지 않고 바로 정리하기 때문에 더 빠르다.
//         using (StreamReader sr = new StreamReader(path)) 
//         using (CsvReader cr = new CsvReader(sr,csvConfig))
//         {
//             var records = cr.GetRecords<Player>(); //CsvHelper가 자동으로 헤더명과 Player 클래스의 속성명을 연결해 객체 리스트를 생성
//             players = records.ToArray();
//         }
//
//         Debug.unityLogger.Log(players.Length.Equals(0) is false
//             ? $"Success Read Players ::: {path}"
//             : $"False Read Players ::: {path}");
//         
//         return players;
//     }*/
//     
//     // public Player.ToTsvPlayerInfo ConvertToTestInfo(Player.PlayerInfo _playerInfo)
//     // {
//     //     Player.ToTsvPlayerInfo testInfo = new Player.ToTsvPlayerInfo
//     //     {
//     //         Key = _playerInfo.key,
//     //         Name = _playerInfo.name,
//     //         CurrentHp = _playerInfo.currentHp,
//     //         MaxHp = _playerInfo.maxHp,
//     //         HasSkills = _playerInfo.hasSkills,
//     //         SkillSlot0 = _playerInfo.skillSlot0,
//     //         SkillSlot1 = _playerInfo.skillSlot1,
//     //         PositionX = _playerInfo.positionX,
//     //         PositionY = _playerInfo.positionY,
//     //         PositionZ = _playerInfo.positionZ,
//     //         AngleX = _playerInfo.angleX,
//     //         AngleY = _playerInfo.angleY,
//     //         AngleZ = _playerInfo.angleZ
//     //     };
//     //
//     //     return testInfo;
//     // }
// }