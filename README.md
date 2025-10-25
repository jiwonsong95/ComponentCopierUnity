## 🎯 Component Field Copier for Unity
### Simplifying component value transfer across similar or inherited classes.

When you create an inherited class and need to transfer configuration values from the base component, manually copying each field or dealing with raw YAML is cumbersome and prone to errors. This simple Editor utility solves that problem by making value synchronization quick and safe.

### simply drag and drop your component, and voila! it's done!

## ✨ Features at a Glance
<img width="382" height="157" alt="image" src="https://github.com/user-attachments/assets/c58ad54a-8983-48b9-9f7e-0c73b06389a8" />

- Seamless Value Transfer: Copies field values from a Source component to a Target component based on matching field names and types.
- Reflection Powered: Utilizes C# Reflection, allowing it to work perfectly for:
- Inherited Classes (e.g., Component2 inheriting from Component1).
- Unrelated Classes that happen to share common configuration fields.
- Handles All Field Types: Copies both Public and Non-Public fields, including those marked with [SerializeField].
- Safe and Simple: Operates entirely within an EditorWindow, providing a safe, visual, drag-and-drop interface.

