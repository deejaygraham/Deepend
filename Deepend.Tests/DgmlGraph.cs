using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Deepend.Tests
{
	[TestClass]
	public class DgmlGraph
	{
		[TestMethod]
		public void Dgml_Can_Create_An_Empty_Document()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsTrue(document.Contains("<DirectedGraph"));
				Assert.IsTrue(document.Contains("</DirectedGraph>"));
				Assert.IsTrue(document.Contains("<Nodes>"));
				Assert.IsTrue(document.Contains("</Nodes>"));
				Assert.IsTrue(document.Contains("<Links>"));
				Assert.IsTrue(document.Contains("</Links>"));
			}
		}

		[TestMethod]
		public void Dgml_Writes_Net_Type_Declaration_And_Namespace()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				IntrospectedType t = new IntrospectedType();
				t.Name = "StringBuilder";
				t.Namespace = "System.Text";

				//t.Generate().ToList().ForEach(x => x.WriteTo(writer));
				//graph.Declare(t);

				//graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsTrue(document.Contains("<Node Id=\"CLS_System.Text.StringBuilder\" Label=\"System.Text.StringBuilder\" />"));
				Assert.IsTrue(document.Contains("<Node Id=\"NS_System.Text\" Label=\"System.Text\" Group=\"Expanded\" />"));
				Assert.IsTrue(document.Contains("<Link Source=\"NS_System.Text\" Target=\"CLS_System.Text.StringBuilder\" Category=\"Contains\" />"));
			}
		}

		[TestMethod]
		public void Dgml_Writes_Custom_Type_Declaration_And_Namespace()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				IntrospectedType t = new IntrospectedType();
				t.Name = "MyClass";
				t.Namespace = "MyNamespace";

//				graph.Declare(t);

				graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyClass\" Label=\"MyClass\" />"));
				Assert.IsTrue(document.Contains("<Node Id=\"NS_MyNamespace\" Label=\"MyNamespace\" Group=\"Expanded\" />"));
				Assert.IsTrue(document.Contains("<Link Source=\"NS_MyNamespace\" Target=\"CLS_MyClass\" Category=\"Contains\" />"));
			}
		}

		[TestMethod]
		public void Dgml_Ignores_Anonymous_Types()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				IntrospectedType t = new IntrospectedType();
				t.Name = "<MyClass";
				t.Namespace = "MyNamespace";

				//graph.Declare(t);

				graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsFalse(document.Contains("<MyClass"));
			}
		}

		[TestMethod]
		public void Dgml_Removes_Ampersand_From_Names()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				IntrospectedType t = new IntrospectedType();
				t.Name = "&MyClass";
				t.Namespace = "MyNamespace";

				//graph.Declare(t);

				graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyClass\" Label=\"MyClass\" />"));
			}
		}

		[TestMethod]
		public void Dgml_Links_Two_Dependent_Types()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				IntrospectedType thisClass = new IntrospectedType();
				thisClass.Name = "&MyClass";
				thisClass.Namespace = "MyNamespace";

				IntrospectedType dependsOnThisClass = new IntrospectedType();
				dependsOnThisClass.Name = "&MyOtherClass";
				dependsOnThisClass.Namespace = "MyNamespace";

				//graph.Declare(thisClass);
				//graph.Declare(dependsOnThisClass);

				//graph.Link(thisClass, dependsOnThisClass, LinkRelationship.Dependency);

				graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyClass\" Label=\"MyClass\" />"));
				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyOtherClass\" Label=\"MyOtherClass\" />"));
				Assert.IsTrue(document.Contains("<Link Source=\"CLS_MyClass\" Target=\"CLS_MyOtherClass\" />"));
			}
		}

		[TestMethod]
		public void Dgml_Links_Class_Hierarchy()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				IntrospectedType subClass = new IntrospectedType();
				subClass.Name = "&MyClass";
				subClass.Namespace = "MyNamespace";

				IntrospectedType superClass = new IntrospectedType();
				superClass.Name = "&MyOtherClass";
				superClass.Namespace = "MyNamespace";

				//graph.Declare(subClass);
				//graph.Declare(superClass);
				//graph.Link(subClass, superClass, LinkRelationship.Inheritance);

				graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyClass\" Label=\"MyClass\" />"));
				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyOtherClass\" Label=\"MyOtherClass\" />"));
				Assert.IsTrue(document.Contains("<Link Source=\"CLS_MyClass\" Target=\"CLS_MyOtherClass\" Label=\"Derived From\" />"));
			}
		}

		[TestMethod]
		public void Dgml_Links_Interface_Implementation()
		{
			IGraphDependencies graph = new DgmlDependencies();

			using (TextWriter writer = new StringWriter())
			{
				IntrospectedType concreteType = new IntrospectedType();
				concreteType.Name = "&MyClass";
				concreteType.Namespace = "MyNamespace";

				IntrospectedType interfaceType = new IntrospectedType();
				interfaceType.Name = "&MyOtherClass";
				interfaceType.Namespace = "MyNamespace";

				//graph.Declare(concreteType);
				//graph.Declare(interfaceType);
				//graph.Link(concreteType, interfaceType, LinkRelationship.Interface);

				graph.SaveTo(writer);

				string document = writer.ToString();

				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyClass\" Label=\"MyClass\" />"));
				Assert.IsTrue(document.Contains("<Node Id=\"CLS_MyOtherClass\" Label=\"MyOtherClass\" />"));
				Assert.IsTrue(document.Contains("<Link Source=\"CLS_MyClass\" Target=\"CLS_MyOtherClass\" Label=\"Implements\" StrokeDashArray=\"1,3\" />"));
			}
		}
	}
}
