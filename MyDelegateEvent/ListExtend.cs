using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDelegateEvent
{
    public class ListExtend
    {
        #region 数据准备
        /// <summary>
        /// 准备数据
        /// </summary>
        /// <returns></returns>
        private List<Student> GetStudentList()
        {
            #region 初始化数据
            List<Student> studentList = new List<Student>()
            {
                new Student()
                {
                    Id=1,
                    Name="老K",
                    ClassId=2,
                    Age=35
                },
                new Student()
                {
                    Id=1,
                    Name="hao",
                    ClassId=2,
                    Age=23
                },
                 new Student()
                {
                    Id=1,
                    Name="大水",
                    ClassId=2,
                    Age=27
                },
                 new Student()
                {
                    Id=1,
                    Name="半醉人间",
                    ClassId=2,
                    Age=26
                },
                new Student()
                {
                    Id=1,
                    Name="风尘浪子",
                    ClassId=2,
                    Age=25
                },
                new Student()
                {
                    Id=1,
                    Name="一大锅鱼",
                    ClassId=2,
                    Age=24
                },
                new Student()
                {
                    Id=1,
                    Name="小白",
                    ClassId=2,
                    Age=21
                },
                 new Student()
                {
                    Id=1,
                    Name="yoyo",
                    ClassId=2,
                    Age=22
                },
                 new Student()
                {
                    Id=1,
                    Name="冰亮",
                    ClassId=2,
                    Age=34
                },
                 new Student()
                {
                    Id=1,
                    Name="瀚",
                    ClassId=2,
                    Age=30
                },
                new Student()
                {
                    Id=1,
                    Name="毕帆",
                    ClassId=2,
                    Age=30
                },
                new Student()
                {
                    Id=1,
                    Name="一点半",
                    ClassId=2,
                    Age=30
                },
                new Student()
                {
                    Id=1,
                    Name="小石头",
                    ClassId=2,
                    Age=28
                },
                new Student()
                {
                    Id=1,
                    Name="大海",
                    ClassId=2,
                    Age=30
                },
                 new Student()
                {
                    Id=3,
                    Name="yoyo",
                    ClassId=3,
                    Age=30
                },
                  new Student()
                {
                    Id=4,
                    Name="unknown",
                    ClassId=4,
                    Age=30
                }
            };
            #endregion
            return studentList;
        }
        #endregion

        public delegate bool ThanDelegate(Student student);
        private bool Than(Student student)
        {
            return student.Age > 25;
        }
        private bool LengthThan(Student student)
        {
            return student.Name.Length > 2;
        }
        private bool AllThan(Student student)
        {
            return student.Name.Length > 2 && student.Age > 25 && student.ClassId == 2;
        }

        public void Show()
        {
            List<Student> studentList = this.GetStudentList();
            {
                ThanDelegate method = new ThanDelegate(this.Than);
                List<Student> result = this.GetListDelegate(studentList, method);
                Console.WriteLine($"结果一共有{result.Count()}个");
            }
            {
                ThanDelegate method = new ThanDelegate(this.LengthThan);
                List<Student> result = this.GetListDelegate(studentList, method);
                Console.WriteLine($"结果一共有{result.Count()}个");
            }
            {
                ThanDelegate method = new ThanDelegate(this.AllThan);
                List<Student> result = this.GetListDelegate(studentList, method);
                Console.WriteLine($"结果一共有{result.Count()}个");
            }
        }

        /// <summary>
        /// 判断逻辑传递进来+实现共用逻辑
        /// 委托解耦，减少重复代码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private List<Student> GetListDelegate(List<Student> source, ThanDelegate method)
        {
            List<Student> result = new List<Student>();
            foreach (Student student in source)
            {
                if (method.Invoke(student))
                {
                    result.Add(student);
                }
            }
            return result;
        }
    }
}
