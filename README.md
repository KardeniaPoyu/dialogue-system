# Unity Dialogue System

## English

### Overview
This Unity Dialogue System is a flexible and extensible framework designed for implementing interactive dialogue in Unity games. It supports dynamic dialogue loading from Excel files, typewriter-style text display with sound effects, speaker visuals with active/inactive states, and choice-based branching dialogues. The system is ideal for narrative-driven games, such as visual novels, RPGs, or adventure games.

### Features
- **Excel Integration**: Import dialogue data from Excel files (.xlsx, .xls) using the `DialogueExcelImporter` script, supporting multilingual text and sprite assignments.
- **Dynamic Dialogue Flow**: Manage dialogue sequences with a `DialogueData` ScriptableObject, including speaker names, sprites, and branching choices.
- **Typewriter Effect**: Display dialogue text with a customizable typewriter effect, accompanied by optional typing sound effects.
- **Speaker Visuals**: Display left and right speaker sprites with active/inactive color states to highlight the current speaker.
- **Choice System**: Support for dialogue choices that can branch to different dialogue sequences or trigger gameplay effects (e.g., morality, coins, health changes).
- **Interactable Dialogues**: Start dialogues via interactable objects in the game world, with configurable interaction prompts and distances.
- **Log System**: Optionally add log entries after specific dialogue nodes for narrative tracking.

### Installation
1. **Clone or Import**: Add the provided scripts to your Unity project's `Assets` folder.
2. **Dependencies**:
   - Ensure `TextMeshPro` (TMPro) is installed via the Unity Package Manager for text rendering.
   - Install the `ExcelDataReader` NuGet package for Excel import functionality (used in `DialogueExcelImporter.cs`).
3. **Setup**:
   - Create a `DialogueDataListSO` asset via the Unity menu (`Assets > Create > 对话/DialogueDataListSO`).
   - Create `DialogueData` assets for individual dialogues (`Assets > Create > 对话/DialogueData`).
   - Configure the `DialogueSystem` component on a GameObject in your scene, assigning the UI components and `DialogueDataListSO`.

### Usage
1. **Excel Setup**:
   - Prepare an Excel file with the following columns (at minimum):
     - `ID`: Unique dialogue ID (integer).
     - `SpeakerNameLeft`: Name of the left speaker.
     - `SpeakerNameRight`: Name of the right speaker.
     - `SpriteLeftPath`: Resource path to the left speaker's sprite.
     - `SpriteRightPath`: Resource path to the right speaker's sprite.
     - `Row`: Speaker position (`left` or `right`).
     - `Speech`: Dialogue text.
     - `ChooseID`: Optional ID for branching choices (integer).
     - `ChooseText`: Optional text for choice buttons.
   - Place the Excel file in `Assets/Resources/DialogueExcel`.
   - Use the Unity Editor menu `Tools > Dialogue > Import Excel (Fixed)` to import the Excel data into `DialogueData` assets.

2. **Scene Setup**:
   - Attach the `DialogueSystem` script to a GameObject.
   - Assign UI elements (e.g., `TMP_Text` for names and dialogue, `Image` for speaker sprites, `Button` for continue/next, and a `Transform` for choice button parenting) in the Inspector.
   - Assign a `DialogueDataListSO` asset containing your dialogue data.
   - Add `DialogueInteractable` components to interactable objects in the scene to trigger dialogues.

3. **Customization**:
   - Adjust `typingSpeed` and `typingSound` in the `DialogueSystem` for the typewriter effect.
   - Modify `activeSpeakerColor` and `inactiveSpeakerColor` to control speaker sprite visuals.
   - Set `keepInactiveVisible` to `true` to keep inactive speakers visible (dimmed) or `false` to hide them.
   - Implement custom effects in `ApplyChoiceEffects` for choice-based gameplay mechanics (e.g., morality, coins, health).

4. **Starting a Dialogue**:
   - Call `DialogueSystem.Instance.StartDialogue(dialogueID)` to begin a dialogue with the specified ID.
   - Interact with objects containing the `DialogueInteractable` component to trigger dialogues in-game.

### Project Structure
- **DialogueData.cs**: Defines the `DialogueData` ScriptableObject for storing dialogue information, including speaker names, sprites, and choices.
- **DialogueExcelImporter.cs**: Handles importing dialogue data from Excel files in the Unity Editor, creating `DialogueData` assets.
- **DialogueSystem.cs**: Core system for managing dialogue flow, UI updates, typewriter effects, and choice handling.
- **DialogueInteractable.cs**: Implements the `IInteractable` interface for triggering dialogues via in-game interactions.
- **DialogueDataListSO.cs**: Manages a list of `DialogueData` assets and a dictionary for quick lookup by ID.
- **DialoguePanel.cs**: Defines the UI components for displaying dialogue text, speaker names, sprites, and choice buttons.

### Notes
- The system assumes the presence of a `Resources/DialogueData` folder for storing generated `DialogueData` assets.
- Ensure sprite assets are placed in the `Resources` folder for `Resources.Load<Sprite>` to work correctly.
- The `DialogueExcelImporter` requires the `ExcelDataReader` library and is only active in the Unity Editor (`#if UNITY_EDITOR`).
- The typewriter effect includes customizable delays for punctuation marks (e.g., commas, periods) to enhance readability.
- The system supports dialogue branching via choices, with effects applied through the `ApplyChoiceEffects` method.

### Limitations
- The Excel importer assumes a specific column structure; deviations may cause parsing errors.
- The system currently supports only left/right speaker positions; additional positions would require extending the `Row` enum.
- Dialogue assets are overwritten during Excel import; ensure backups if manual edits are made.
- The typewriter sound effect requires an `AudioClip` and an `AudioSource` component on the `DialogueSystem` GameObject.

### Future Improvements
- Add support for animated speaker sprites or transitions.
- Implement a dialogue editor window for manual dialogue creation in Unity.
- Support for multi-language dialogues by extending the Excel importer.
- Add event triggers for specific dialogue nodes (e.g., animations, scene changes).
- Optimize memory usage for large dialogue datasets.

### License
This project is provided as-is for educational and non-commercial use. Feel free to modify and extend it for your own projects.

---

## 中文

### 概述
此 Unity 对话系统是一个灵活且可扩展的框架，专为在 Unity 游戏中实现交互式对话而设计。它支持从 Excel 文件动态加载对话数据、打字机风格的文本显示（带音效）、具有活跃/非活跃状态的角色立绘展示，以及基于选项的对话分支。该系统非常适合叙事驱动的游戏，如视觉小说、RPG 或冒险游戏。

### 功能
- **Excel 集成**：通过 `DialogueExcelImporter` 脚本从 Excel 文件（.xlsx、.xls）导入对话数据，支持多语言文本和角色立绘分配。
- **动态对话流程**：通过 `DialogueData` ScriptableObject 管理对话序列，包括角色名称、立绘和分支选项。
- **打字机效果**：以可自定义的打字机效果显示对话文本，并支持可选的打字音效。
- **角色立绘**：显示左侧和右侧角色立绘，通过活跃/非活跃颜色状态高亮当前说话者。
- **选项系统**：支持对话选项，可分支到不同的对话序列或触发游戏效果（例如道德值、金币、生命值变化）。
- **交互式对话**：通过游戏世界中的可交互对象启动对话，支持配置交互提示和距离。
- **日志系统**：在特定对话节点后可选地添加日志条目，用于叙事跟踪。

### 安装
1. **克隆或导入**：将提供的脚本添加到 Unity 项目的 `Assets` 文件夹。
2. **依赖项**：
   - 通过 Unity 包管理器安装 `TextMeshPro` (TMPro)，用于文本渲染。
   - 安装 `ExcelDataReader` NuGet 包，用于 Excel 导入功能（在 `DialogueExcelImporter.cs` 中使用）。
3. **设置**：
   - 通过 Unity 菜单创建 `DialogueDataListSO` 资源（`Assets > Create > 对话/DialogueDataListSO`）。
   - 为单独对话创建 `DialogueData` 资源（`Assets > Create > 对话/DialogueData`）。
   - 在场景中的 GameObject 上配置 `DialogueSystem` 组件，分配 UI 组件和 `DialogueDataListSO`。

### 使用方法
1. **Excel 设置**：
   - 准备一个包含以下列（至少）的 Excel 文件：
     - `ID`：唯一对话 ID（整数）。
     - `SpeakerNameLeft`：左侧说话者名称。
     - `SpeakerNameRight`：右侧说话者名称。
     - `SpriteLeftPath`：左侧角色立绘的资源路径。
     - `SpriteRightPath`：右侧角色立绘的资源路径。
     - `Row`：说话者位置（`left` 或 `right`）。
     - `Speech`：对话文本。
     - `ChooseID`：可选的分支选项 ID（整数）。
     - `ChooseText`：可选的选项按钮文本。
   - 将 Excel 文件放置在 `Assets/Resources/DialogueExcel` 中。
   - 使用 Unity 编辑器菜单 `Tools > Dialogue > Import Excel (Fixed)` 导入 Excel 数据到 `DialogueData` 资源。

2. **场景设置**：
   - 将 `DialogueSystem` 脚本附加到 GameObject 上。
   - 在 Inspector 中分配 UI 元素（例如，`TMP_Text` 用于名称和对话，`Image` 用于角色立绘，`Button` 用于继续/下一句，`Transform` 用于选项按钮的父节点）。
   - 分配包含对话数据的 `DialogueDataListSO` 资源。
   - 在场景中的可交互对象上添加 `DialogueInteractable` 组件以触发对话。

3. **自定义**：
   - 在 `DialogueSystem` 中调整 `typingSpeed` 和 `typingSound` 以控制打字机效果。
   - 修改 `activeSpeakerColor` 和 `inactiveSpeakerColor` 以控制角色立绘的视觉效果。
   - 将 `keepInactiveVisible` 设置为 `true` 以保持非活跃角色可见（变暗），或 `false` 以隐藏。
   - 在 `ApplyChoiceEffects` 中实现基于选项的自定义游戏机制（例如道德值、金币、生命值）。

4. **启动对话**：
   - 调用 `DialogueSystem.Instance.StartDialogue(dialogueID)` 以指定 ID 开始对话。
   - 通过与包含 `DialogueInteractable` 组件的对象交互来触发游戏中的对话。

### 项目结构
- **DialogueData.cs**：定义存储对话信息的 `DialogueData` ScriptableObject，包括角色名称、立绘和选项。
- **DialogueExcelImporter.cs**：在 Unity 编辑器中处理从 Excel 文件导入对话数据，创建 `DialogueData` 资源。
- **DialogueSystem.cs**：核心系统，用于管理对话流程、UI 更新、打字机效果和选项处理。
- **DialogueInteractable.cs**：实现 `IInteractable` 接口，用于通过游戏交互触发对话。
- **DialogueDataListSO.cs**：管理 `DialogueData` 资源列表和按 ID 快速查找的字典。
- **DialoguePanel.cs**：定义用于显示对话文本、角色名称、立绘和选项按钮的 UI 组件。

### 注意事项
- 系统假定存在 `Resources/DialogueData` 文件夹用于存储生成的 `DialogueData` 资源。
- 确保立绘资源放置在 `Resources` 文件夹中，以便 `Resources.Load<Sprite>` 正常工作。
- `DialogueExcelImporter` 需要 `ExcelDataReader` 库，仅在 Unity 编辑器中生效（`#if UNITY_EDITOR`）。
- 打字机效果包括针对标点符号（例如逗号、句号）的可自定义延迟，以提高可读性。
- 系统通过 `ApplyChoiceEffects` 方法支持基于选项的对话分支。

### 限制
- Excel 导入器假定特定的列结构；偏差可能导致解析错误。
- 系统当前仅支持左侧/右侧说话者位置；添加其他位置需要扩展 `Row` 枚举。
- Excel 导入期间对话资源会被覆盖；如有手动编辑，请确保备份。
- 打字机音效需要 `AudioClip` 和 `DialogueSystem` GameObject 上的 `AudioSource` 组件。

### 未来改进
- 添加对动画角色立绘或过渡效果的支持。
- 在 Unity 中实现对话编辑器窗口以手动创建对话。
- 通过扩展 Excel 导入器支持多语言对话。
- 为特定对话节点添加事件触发（例如动画、场景切换）。
- 优化大型对话数据集的内存使用。

### 许可
本项目仅限教育和非商业用途，欢迎根据您的项目需求进行修改和扩展。

---

## 日本語

### 概要
この Unity ダイアログシステムは、Unity ゲームで対話的なダイアログを実装するために設計された柔軟で拡張可能なフレームワークです。Excel ファイルからの動的なダイアログデータの読み込み、タイプライター形式のテキスト表示（効果音付き）、アクティブ/非アクティブ状態のスピーカー画像の表示、選択肢に基づく分岐ダイアログをサポートしています。このシステムは、ビジュアルノベル、RPG、アドベンチャーゲームなどの物語主導のゲームに最適です。

### 機能
- **Excel 統合**：`DialogueExcelImporter` スクリプトを使用して Excel ファイル（.xlsx、.xls）からダイアログデータをインポートし、多言語テキストとスプライト割り当てをサポート。
- **動的ダイアログフロー**：`DialogueData` ScriptableObject を使用して、スピーカー名、スプライト、分岐選択肢を含むダイアログシーケンスを管理。
- **タイプライター効果**：カスタマイズ可能なタイプライター効果でダイアログテキストを表示し、オプションでタイプ音効果を付加。
- **スピーカー画像**：左右のスピーカースプライトを表示し、アクティブ/非アクティブの色状態で現在のスピーカーを強調。
- **選択肢システム**：異なるダイアログシーケンスへの分岐やゲームプレイ効果（例：道徳値、コイン、ヘルスの変更）をトリガーするダイアログ選択肢をサポート。
- **対話可能なダイアログ**：ゲームワールド内の対話可能なオブジェクトを介してダイアログを開始し、インタラクションプロンプトと距離を設定可能。
- **ログシステム**：特定のダイアログノード後にオプションでログエントリを追加し、物語の追跡をサポート。

### インストール
1. **クローンまたはインポート**：提供されたスクリプトを Unity プロジェクトの `Assets` フォルダに追加。
2. **依存関係**：
   - テキストレンダリングのために Unity パッケージマネージャー経由で `TextMeshPro` (TMPro) をインストール。
   - Excel インポート機能（`DialogueExcelImporter.cs` で使用）に必要な `ExcelDataReader` NuGet パッケージをインストール。
3. **設定**：
   - Unity メニュー（`Assets > Create > 对话/DialogueDataListSO`）から `DialogueDataListSO` アセットを作成。
   - 個々のダイアログ用に `DialogueData` アセットを作成（`Assets > Create > 对话/DialogueData`）。
   - シーン内の GameObject に `DialogueSystem` コンポーネントを配置し、UI コンポーネントと `DialogueDataListSO` を割り当て。

### 使用方法
1. **Excel の設定**：
   - 以下の列（少なくとも）を含む Excel ファイルを準備：
     - `ID`：ユニークなダイアログ ID（整数）。
     - `SpeakerNameLeft`：左側のスピーカー名。
     - `SpeakerNameRight`：右側のスピーカー名。
     - `SpriteLeftPath`：左側のスピーカースプライトのリソースパス。
     - `SpriteRightPath`：右側のスピーカースプライトのリソースパス。
     - `Row`：スピーカーの位置（`left` または `right`）。
     - `Speech`：ダイアログテキスト。
     - `ChooseID`：オプションの分岐選択肢 ID（整数）。
     - `ChooseText`：オプションの選択肢ボタンテキスト。
   - Excel ファイルを `Assets/Resources/DialogueExcel` に配置。
   - Unity エディターメニュー `Tools > Dialogue > Import Excel (Fixed)` を使用して Excel データを `DialogueData` アセットにインポート。

2. **シーンの設定**：
   - `DialogueSystem` スクリプトを GameObject にアタッチ。
   - インスペクターで UI 要素（例：`TMP_Text`（名前とダイアログ用）、`Image`（スピーカースプライト用）、`Button`（続ける/次へ用）、`Transform`（選択肢ボタンの親用））を割り当て。
   - ダイアログデータを含む `DialogueDataListSO` アセットを割り当て。
   - シーン内の対話可能なオブジェクトに `DialogueInteractable` コンポーネントを追加してダイアログをトリガー。

3. **カスタマイズ**：
   - `DialogueSystem` の `typingSpeed` と `typingSound` を調整してタイプライター効果を制御。
   - `activeSpeakerColor` と `inactiveSpeakerColor` を変更してスピーカースプライトの視覚効果を制御。
   - `keepInactiveVisible` を `true` に設定して非アクティブなスピーカーを表示（暗くする）、`false` に設定して非表示。
   - `ApplyChoiceEffects` で選択肢に基づくカスタムゲームメカニクス（例：道徳値、コイン、ヘルス）を実装。

4. **ダイアログの開始**：
   - `DialogueSystem.Instance.StartDialogue(dialogueID)` を呼び出して指定された ID のダイアログを開始。
   - `DialogueInteractable` コンポーネントを含むオブジェクトと対話してゲーム内でダイアログをトリガー。

### プロジェクト構造
- **DialogueData.cs**：スピーカー名、スプライト、選択肢を含むダイアログ情報を格納する `DialogueData` ScriptableObject を定義。
- **DialogueExcelImporter.cs**：Unity エディターで Excel ファイルからダイアログデータをインポートし、`DialogueData` アセットを作成。
- **DialogueSystem.cs**：ダイアログフロー、UI 更新、タイプライター効果、選択肢処理を管理するコアシステム。
- **DialogueInteractable.cs**：ゲーム内インタラクションを介してダイアログをトリガーするための `IInteractable` インターフェースを実装。
- **DialogueDataListSO.cs**：`DialogueData` アセットのリストと ID による高速検索用の辞書を管理。
- **DialoguePanel.cs**：ダイアログテキスト、スピーカー名、スプライト、選択肢ボタンを表示するための UI コンポーネントを定義。

### 注意
- システムは生成された `DialogueData` アセットを格納するために `Resources/DialogueData` フォルダの存在を前提としています。
- `Resources.Load<Sprite>` が正しく動作するように、スプライトアセットを `Resources` フォルダに配置してください。
- `DialogueExcelImporter` は `ExcelDataReader` ライブラリを必要とし、Unity エディターでのみ動作します（`#if UNITY_EDITOR`）。
- タイプライター効果は、句読点（例：カンマ、ピリオド）に対するカスタマイズ可能な遅延を含み、読みやすさを向上させます。
- システムは `ApplyChoiceEffects` メソッドを通じて選択肢に基づくダイアログ分岐をサポートします。

### 制限
- Excel インポーターは特定の列構造を前提としており、逸脱すると解析エラーが発生する可能性があります。
- システムは現在、左/右のスピーカー位置のみをサポートしており、追加の位置には `Row` 列挙型の拡張が必要です。
- Excel インポート中にダイアログアセットが上書きされるため、手動編集がある場合はバックアップを確保してください。
- タイプライター音効果には `AudioClip` と `DialogueSystem` GameObject 上の `AudioSource` コンポーネントが必要です。

### 今後の改善
- アニメーション付きスピーカースプライトやトランジションのサポートを追加。
- Unity 内で手動ダイアログ作成用のダイアログエディターウィンドウを実装。
- Excel インポーターを拡張して多言語ダイアログをサポート。
- 特定のダイアログノードに対するイベントトリガー（例：アニメーション、シーン変更）を追加。
- 大規模なダイアログデータセットのメモリ使用を最適化。

### ライセンス
このプロジェクトは教育および非商業用途向けに提供されます。ご自身のプロジェクト向けに自由に修正および拡張してください。
