import re

file_path = r"d:\Codes\Bulk-Crap-Uninstaller\source\BulkCrapUninstaller\Forms\Windows\SettingsWindow.Designer.cs"

with open(file_path, 'r', encoding='utf-8') as f:
    lines = f.readlines()

new_lines = []
# Patterns to match lines to comment out
# Matches: this.checkBoxX.UseVisualStyleBackColor = true;
# Matches: this.radioButtonX.UseVisualStyleBackColor = true;
# Matches: this.buttonX.UseVisualStyleBackColor = true;
# Matches: this.tabPageX.UseVisualStyleBackColor = true;
# Matches: this.comboBoxX.FormattingEnabled = true;
# Matches: this.textBoxX.Multiline = true; (if existing)
pattern = re.compile(r'^\s*this\.(checkBox|radioButton|button|tabPage|textBox|comboBox)\w+\.(UseVisualStyleBackColor|FormattingEnabled)\s*=\s*(true|false);')

for line in lines:
    if pattern.match(line):
        new_lines.append("// " + line)
    else:
        new_lines.append(line)

with open(file_path, 'w', encoding='utf-8') as f:
    f.writelines(new_lines)

print("Cleaned UseVisualStyleBackColor from " + file_path)
