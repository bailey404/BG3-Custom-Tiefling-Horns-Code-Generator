Imports System.Diagnostics
Imports System.IO
Imports System.Text

Public Class HCG

    Public Function GenUUID() As String 'This function generates the UUID, converts it to a string, and returns that string as the output value.
        Dim gennedUUID As Guid = Guid.NewGuid()
        Dim gennedUUIDAsString As String = gennedUUID.ToString()

        Return gennedUUIDAsString
    End Function

    Public Function GenHandle() As String 'This function is like the GenUUID function but specifically for the TranslationString handle because it's a weird format.
        Dim initialUUID As Guid = Guid.NewGuid()
        Dim UUIDAsString As String = initialUUID.ToString()
        Dim convertedToSpecialSnowflake As String = Replace(UUIDAsString, "-", "f") 'The format of the TranslationString handle is a regular UUID but the dashes (-) are replaced with fs.
        Dim convertedToHandle As String = "h" & convertedToSpecialSnowflake 'And they all have h added to the beginning too. Probably could have done this combined with the last step but eh.

        Return convertedToHandle
    End Function

    Public Function GetFilePath(filterType) As String 'This is to get the file path needed for the snippet generation. It returns the path to the chosen file starting inside the "Generated" folder.
        Dim filePath As String
        Using openFileDialog = New OpenFileDialog()
            If filterType = "DDS" Then
                openFileDialog.Filter = "DDS Files (*.dds)|*.dds"
            End If
            If filterType = "GR2" Then
                openFileDialog.Filter = "GR2 Files (*.GR2)|*.GR2"
            End If
            If openFileDialog.ShowDialog() = DialogResult.OK Then
                filePath = openFileDialog.FileName
                Dim filePathArray() As String = Split(filePath, Delimiter:="Generated")
                Return "Generated" & filePathArray(1) 'I'm sure there's a way to concatenate in the declaration but that optimization is low priority right now.
            End If
        End Using
    End Function

    Public Function ClearAllInput() 'Exactly what it says on the tin.
        tboxInputMeshUUID.Text = ""
        tboxInputMeshPrefix.Text = ""
        tboxInputTexturePrefix.Text = ""
        DisplayedNameInputBox.Text = ""
        ModAuthorInputBox.Text = ""
        ModDescInputBox.Text = ""
        ModFolderInputBox.Text = ""
        ModNameInputBox.Text = ""

        If MaterialUUIDLockPBox.Tag = "unlocked" Then
            tboxInputMaterialUUID.Text = ""
        End If
        If PMapUUIDLockPBox.Tag = "unlocked" Then
            tboxInputPMapUUID.Text = ""
        End If
        If BMapUUIDLockPBox.Tag = "unlocked" Then
            tboxInputBMapUUID.Text = ""
        End If
        If NMapUUIDLockPBox.Tag = "unlocked" Then
            tboxInputNMapUUID.Text = ""
        End If
        If MMapUUIDLockPBox.Tag = "unlocked" Then
            tboxInputMMapUUID.Text = ""
        End If
        If CCOptionUUIDLockPBox.Tag = "unlocked" Then
            CCOptionUUIDInputBox.Text = ""
        End If
        If NameHandleUUIDLockPBox1.Tag = "unlocked" Then
            NameHandleInputBox1.Text = ""
        End If
        If ModUUIDLockPBox.Tag = "unlocked" Then
            ModUUIDInputBox.Text = ""
        End If
    End Function

    Public Function ClearFilePaths() 'Exactly what it says on the tin.
        PMapPathBox.Text = ""
        BMapPathBox.Text = ""
        NMapPathBox.Text = ""
        MMapPathBox.Text = ""
        GR2FilePathBox.Text = ""
    End Function

    Public Function ClearAllSnippets() 'Exactly what it says on the tin.
        MergedMaterialOutputBox.Text = ""
        MergedMeshOutputBox.Text = ""
        MergedTextureOutputBox.Text = ""
        CCAVOutputBox.Text = ""
        LocaOutputBox.Text = ""
        MetaOutputRTBox.Text = ""
    End Function

    Public Function GetRaceUUID(raceName) 'Returns the specific race UUID for the selected drop-down option.
        Dim raceUUID = "NULL"

        Select Case raceName
            Case "Human"
                raceUUID = "0eb594cb-8820-4be6-a58d-8be7a1a98fba"
            Case "Githyanki"
                raceUUID = "bdf9b779-002c-4077-b377-8ea7c1faa795"
            Case "Tiefling"
                raceUUID = "b6dccbed-30f3-424b-a181-c4540cf38197"
            Case "Elf"
                raceUUID = "6c038dcb-7eb5-431d-84f8-cecfaf1c0c5a"
            Case "Half-Elf"
                raceUUID = "45f4ac10-3c89-4fb2-b37d-f973bb9110c0"
            Case "Dwarf"
                raceUUID = "0ab2874d-cfdc-405e-8a97-d37bfbb23c52"
            Case "Halfling"
                raceUUID = "78cd3bcc-1c43-4a2a-aa80-c34322c16a04"
            Case "Gnome"
                raceUUID = "f1b3f884-4029-4f0f-b158-1f9fe0ae5a0d"
            Case "Drow"
                raceUUID = "4f5d1434-5175-4fa9-b7dc-ab24fba37929"
            Case "Dragonborn"
                raceUUID = "9c61a74a-20df-4119-89c5-d996956b6c66"
            Case "Half-Orc"
                raceUUID = "5c39a726-71c8-4748-ba8d-f768b3c11a91"
        End Select

        Return raceUUID
    End Function

    Public Function GenLocaSnip(handleInput, nameInput)
        Dim inputFile As String = My.Resources.SnippetResources.locaSnippetInput 'Pulls the template from the resX file.
        Dim replaceHandle = inputFile.Replace("UUIDhandle", handleInput) 'Replaces the TranslationString handle placeholder with the input handle.
        Dim replaceName = replaceHandle.Replace("OPTION_NAME", nameInput) 'Replaces the Option Name placeholder with the input Display Name.

        LocaOutputBox.Text = replaceName 'Shows the converted template in the output window.
    End Function

    Public Function GenMetaText(modUUIDInput, modAuthorInput, modDescInput, modFolderInput, modNameInput)
        Dim inputFile As String = My.Resources.SnippetResources.metaSnippetInput 'Pulls the template from the resX file.
        Dim replaceUUID = inputFile.Replace("MOD_UUID", modUUIDInput) 'Replaces the UUID placeholder with the input UUID.
        Dim replaceAuthor = replaceUUID.Replace("AUTHOR_NAME", modAuthorInput) 'Replaces the mod author placeholder with the input author.
        Dim replaceDesc = replaceAuthor.Replace("MOD_DESC", modDescInput) 'Replaces the description placeholder with the input description.
        Dim replaceFolder = replaceDesc.Replace("FOLDER_NAME", modFolderInput) 'Replaces the folder placeholder with the input folder name.
        Dim replaceModName = replaceFolder.Replace("MOD_NAME", modNameInput) 'Replace the mod name placeholder with the input mod name.

        MetaOutputRTBox.Text = replaceModName 'Shows the converted template in the output window.
    End Function

    Public Function GenCCAVSnippet(meshUUIDInput, CCUUIDInput, handleInput, bodyShapeInput, bodyTypeInput, slotInput, raceInput)
        Dim inputFile As String = My.Resources.SnippetResources.CCVASnippetInput 'Pulls the template from the resX file.
        Dim replaceMeshUUID = inputFile.Replace("MESH_UUID", meshUUIDInput) 'Replaces the mesh UUID placeholder with the input UUID.
        Dim replaceCCUUID = replaceMeshUUID.Replace("CCAVUUID", CCUUIDInput) 'Replace the CCAV UUID placeholder with the input UUID.
        Dim replaceHandle = replaceCCUUID.Replace("UUIDhandle", handleInput) 'Replaces the TranslationString handle placeholder with the input handle.
        Dim replaceBodyShape = replaceHandle.Replace("B_SHAPE", bodyShapeInput) 'Replaces the bshape placeholder based on the radio button chosen.
        Dim replaceBodyType = replaceBodyShape.Replace("B_TYPE", bodyTypeInput) 'Replace the btype placeholder based on the radio button chosen.
        Dim replaceSlot = replaceBodyType.Replace("CC_SLOT", slotInput) 'Replace the slot placeholder based on the chosen drop-down option.
        Dim replaceRace = replaceSlot.Replace("RACE_UUID", GetRaceUUID(raceInput)) 'Replace the race placeholder based on the drop-down option, calling the getRaceUUID function to return the UUID based on provided race.

        CCAVOutputBox.Text = replaceRace 'Shows the converted template in the output window.
    End Function

    Public Function GenMergedMaterialSnippet(handleInput, materialUUIDInput, texturePrefixInput, nMapUUIDInput, bMapUUIDInput, mMapUUIDInput, pMapUUIDInput)
        Dim inputFile As String = My.Resources.SnippetResources.mergedMaterialSnippetInput 'Pulls the template from the resX file.
        Dim replaceHandle = inputFile.Replace("UUIDhandle", handleInput) 'Replaces the TranslationString handle placeholder with the input handle.
        Dim replaceMaterialUUID = replaceHandle.Replace("MATERIAL_UUID", materialUUIDInput) 'Replaces the material UUID placeholder with the input UUID.
        Dim replaceTexturePrefix = replaceMaterialUUID.Replace("TEXTURE_PREFIX", texturePrefixInput) 'Replaces the texture prefix placeholder with the input prefix.
        Dim replaceNMapUUID = replaceTexturePrefix.Replace("NMAP_UUID", nMapUUIDInput) 'Replace the Normal Map UUID placeholder with the input UUID.
        Dim replaceBMapUUID = replaceNMapUUID.Replace("BMAP_UUID", bMapUUIDInput) 'Replace the Base Color Map UUID placeholder with the input UUID.
        Dim replaceMMapUUID = replaceBMapUUID.Replace("MMAP_UUID", mMapUUIDInput) 'Replace the Gradient Mask Map UUID placeholder with the input UUID.
        Dim replacePMapUUID = replaceMMapUUID.Replace("PMAP_UUID", pMapUUIDInput) 'Replace the Physical Map UUID placeholder with the input UUID.

        MergedMaterialOutputBox.Text = replacePMapUUID 'Shows the converted template in the output window.
    End Function

    Public Function GenMergedTextureSnippet(handleInput, pMapUUIDInput, texturePrefixInput, pMapPathInput, bMapUUIDInput, bMapPathInput, nMapUUIDInput, nMapPathInput, mMapUUIDInput, mMapPathInput)
        Dim inputFile As String = My.Resources.SnippetResources.mergedTextureSnippetInput 'Pulls the template from the resX file.
        Dim replaceHandle = inputFile.Replace("UUIDhandle", handleInput) 'Replaces the TranslationString handle placeholder with the input handle.
        Dim replaceTexturePrefix = replaceHandle.Replace("TEXTURE_PREFIX", texturePrefixInput) 'Replaces the texture prefix placeholder with the input prefix.
        Dim replaceNMapUUID = replaceTexturePrefix.Replace("NMAP_UUID", nMapUUIDInput) 'Replace the Normal Map UUID placeholder with the input UUID.
        Dim replaceBMapUUID = replaceNMapUUID.Replace("BMAP_UUID", bMapUUIDInput) 'Replace the Base Color Map UUID placeholder with the input UUID.
        Dim replaceMMapUUID = replaceBMapUUID.Replace("MMAP_UUID", mMapUUIDInput) 'Replace the Gradient Mask Map UUID placeholder with the input UUID.
        Dim replacePMapUUID = replaceMMapUUID.Replace("PMAP_UUID", pMapUUIDInput) 'Replace the Physical Map UUID placeholder with the input UUID.
        Dim replacePMapPath = replacePMapUUID.Replace("PMAP_PATH", pMapPathInput) 'Replace the Physical Map filepath placeholder with the input filepath.
        Dim replaceBMapPath = replacePMapPath.Replace("BMAP_PATH", bMapPathInput) 'Replace the Base Color Map filepath placeholder with the input filepath.
        Dim replaceNMapPath = replaceBMapPath.Replace("NMAP_PATH", nMapPathInput) 'Replace the Normal Map filepath placeholder with the input filepath.
        Dim replaceMMapPath = replaceNMapPath.Replace("MMAP_PATH", mMapPathInput) 'Replace the Gradient Mask Map filepath placeholder with the input filepath.

        MergedTextureOutputBox.Text = replaceMMapPath
    End Function

    Public Function GenMergedMeshSnippet(handleInput, meshUUIDInput, meshPrefixInput, GR2PathInput, materialUUIDInput)
        Dim filePath As String = GR2PathInput.Substring(0, GR2PathInput.LastIndexOf("\")) 'Making the dummy path here to prevent unforeseen replacements in the main file. Trimming at the last \.
        Dim dummyPath = filePath & "\" & meshPrefixInput & ".Dummy_Root.0" 'Adding in the lost \, mesh prefix, and dummy root ending.
        Dim inputFile As String = My.Resources.SnippetResources.mergedMeshSnippetInput 'Pulls the template from the resX file.
        Dim replaceHandle = inputFile.Replace("UUIDhandle", handleInput) 'Replaces the TranslationString handle placeholder with the input handle.
        Dim replaceMeshUUID = replaceHandle.Replace("MESH_UUID", meshUUIDInput) 'Replaces the mesh UUID placeholder with the input UUID.
        Dim replaceMeshPrefix = replaceMeshUUID.Replace("MESH_PREFIX", meshPrefixInput) 'Replaces the mesh prefix/filename prefix placeholder with the input prefix.
        Dim replaceGR2Path = replaceMeshPrefix.Replace("GR2_PATH", GR2PathInput) 'Replaces the GR2 path placeholder with the input path.
        Dim replaceMaterialUUID = replaceGR2Path.Replace("MATERIAL_UUID", materialUUIDInput) 'Replaces the material UUID placeholder with the input UUID.
        Dim replaceDummyPath = replaceMaterialUUID.Replace("PATH_DUMMY", dummyPath) 'Replaces the dummy path placeholder with the actual dummy path.

        MergedMeshOutputBox.Text = replaceDummyPath
    End Function

    Private Sub genIDsButton_Click(sender As Object, e As EventArgs) Handles btnGenIDs.Click 'Just calls the functions to generate the UUIDs :)
        If MergedIDGenCBox.Checked Then
            tboxInputMeshUUID.Text = GenUUID()
            If MaterialUUIDLockPBox.Tag = "unlocked" Then
                tboxInputMaterialUUID.Text = GenUUID()
            End If
            If PMapUUIDLockPBox.Tag = "unlocked" Then
                tboxInputPMapUUID.Text = GenUUID()
            End If
            If BMapUUIDLockPBox.Tag = "unlocked" Then
                tboxInputBMapUUID.Text = GenUUID()
            End If
            If NMapUUIDLockPBox.Tag = "unlocked" Then
                tboxInputNMapUUID.Text = GenUUID()
            End If
            If MMapUUIDLockPBox.Tag = "unlocked" Then
                tboxInputMMapUUID.Text = GenUUID()
            End If

        End If
        If CCAVIDGenCBox.Checked Then
            If MergedIDGenCBox.Checked = False Then 'Idiot-proofing so it isn't trying to gen the Mesh UUID twice. Probably wouldn't cause a problem but it would annoy me.
                MeshUUIDInputBox2.Text = GenUUID()
            End If
            If CCOptionUUIDLockPBox.Tag = "unlocked" Then
                CCOptionUUIDInputBox.Text = GenUUID()
            End If
            If NameHandleUUIDLockPBox1.Tag = "unlocked" Then
                NameHandleInputBox1.Text = GenHandle()
            End If
        End If
        If LocaIDGenCBox.Checked Then
            If CCAVIDGenCBox.Checked = False Then 'See previous comment.
                If NameHandleUUIDLockPBox2.Tag = "unlocked" Then
                    NameHandleInputBox2.Text = GenHandle()
                End If
            End If
        End If
        If MetaIDGenCBox.Checked Then
            If ModUUIDLockPBox.Tag = "unlocked" Then
                ModUUIDInputBox.Text = GenUUID()
            End If
        End If
    End Sub

    Private Sub meshUUIDInputBox1_TextChanged(sender As Object, e As EventArgs) Handles tboxInputMeshUUID.TextChanged 'This and the following sub are to make sure that changes in one Mesh UUID field is reflected in the opposite one, since they need to be identical.
        MeshUUIDInputBox2.Text = tboxInputMeshUUID.Text
    End Sub

    Private Sub meshUUIDInputBox2_TextChanged(sender As Object, e As EventArgs) Handles MeshUUIDInputBox2.TextChanged
        tboxInputMeshUUID.Text = MeshUUIDInputBox2.Text
    End Sub

    Private Sub NameHandleInputBox1_TextChanged(sender As Object, e As EventArgs) Handles NameHandleInputBox1.TextChanged 'This and the following is the same, but with the TranlationString/Name Handle field.
        NameHandleInputBox2.Text = NameHandleInputBox1.Text
    End Sub

    Private Sub nameHandleInputBox2_TextChanged(sender As Object, e As EventArgs) Handles NameHandleInputBox2.TextChanged
        NameHandleInputBox1.Text = NameHandleInputBox2.Text
    End Sub

    Private Sub pMapBrowseButton_Click(sender As Object, e As EventArgs) Handles btnPMapBrowse.Click 'Basically just calls the function to populate the file path needed for adding to the snippets.
        Dim filePath = GetFilePath("DDS")
        PMapPathBox.Text = filePath.Replace("\", "/")
    End Sub

    Private Sub bMapBrowseButton_Click(sender As Object, e As EventArgs) Handles btnBMapBrowse.Click 'Basically just calls the function to populate the file path needed for adding to the snippets.
        Dim filePath = GetFilePath("DDS")
        BMapPathBox.Text = filePath.Replace("\", "/")
    End Sub

    Private Sub nMapBrowseButton_Click(sender As Object, e As EventArgs) Handles btnNMapBrowse.Click 'Basically just calls the function to populate the file path needed for adding to the snippets.
        Dim filePath = GetFilePath("DDS")
        NMapPathBox.Text = filePath.Replace("\", "/")
    End Sub

    Private Sub mMapBrowseButton_Click(sender As Object, e As EventArgs) Handles btnMMapBrowse.Click 'Basically just calls the function to populate the file path needed for adding to the snippets.
        Dim filePath = GetFilePath("DDS")
        MMapPathBox.Text = filePath.Replace("\", "/")
    End Sub

    Private Sub GR2FileBrowseButton_Click(sender As Object, e As EventArgs) Handles btnGR2Browse.Click 'Basically just calls the function to populate the file path needed for adding to the snippets.
        Dim filePath = GetFilePath("GR2")
        GR2FilePathBox.Text = filePath.Replace("\", "/")
    End Sub

    Private Sub clearAllInputButton_Click(sender As Object, e As EventArgs) Handles btnClearAllInput.Click
        If MessageBox.Show("This will clear ALL input fields, are you sure?", "Confirm Clear", MessageBoxButtons.YesNo) = DialogResult.Yes Then 'Pops up a confirmation dialog before clearing.
            ClearAllInput()
        End If
    End Sub

    Private Sub clearFilePathsButton_Click(sender As Object, e As EventArgs) Handles btnClearFilePaths.Click
        If MessageBox.Show("This will clear ALL filepath fields, are you sure?", "Confirm Clear", MessageBoxButtons.YesNo) = DialogResult.Yes Then 'Pops up a confirmation dialog before clearing.
            ClearFilePaths()
        End If
    End Sub

    Private Sub clearSnipsButton_Click(sender As Object, e As EventArgs) Handles btnClearAllSnips.Click
        If MessageBox.Show("This will clear ALL output fields, are you sure?", "Confirm Clear", MessageBoxButtons.YesNo) = DialogResult.Yes Then 'Pops up a confirmation dialog before clearing.
            ClearAllSnippets()
        End If
    End Sub

    Private Sub HCG_Load(sender As Object, e As EventArgs) Handles MyBase.Load 'Sets the slot to "horns" and the race to "tiefling" at launch since they're the only options right now. Will probably remove as scope expands.
        cboxSlot.SelectedIndex = 0
        cboxRace.SelectedIndex = 0
        OneClickCopySettingsMenuItem.Checked = False
    End Sub

    Private Sub genLocaSnipButton_Click(sender As Object, e As EventArgs) Handles btnGenLocaSnip.Click
        GenLocaSnip(NameHandleInputBox2.Text, DisplayedNameInputBox.Text)
    End Sub

    Private Sub genMetaSnipButton_Click(sender As Object, e As EventArgs) Handles btnGenMetaSnip.Click
        GenMetaText(ModUUIDInputBox.Text, ModAuthorInputBox.Text, ModDescInputBox.Text, ModFolderInputBox.Text, ModNameInputBox.Text)
    End Sub

    Private Sub genCCAVSnipButton_Click(sender As Object, e As EventArgs) Handles btnGenCCAVSnip.Click
        Dim bodyShape As String
        Dim bodyType As String
        Dim slot = cboxSlot.SelectedItem
        Dim race = cboxRace.SelectedItem

        If StandardBSRButton.Checked Then 'Standard
            bodyShape = "0"
        End If
        If StrongBSRButton.Checked Then 'Strong
            bodyShape = "1"
        End If

        If MascBTRButton.Checked Then 'Masc
            bodyType = "0"
        End If
        If FemBTRButton.Checked Then 'Fem
            bodyType = "1"
        End If

        GenCCAVSnippet(MeshUUIDInputBox2.Text, CCOptionUUIDInputBox.Text, NameHandleInputBox1.Text, bodyShape, bodyType, slot, race)
    End Sub

    Private Sub genMergedSnipButton_Click(sender As Object, e As EventArgs) Handles btnGenMergedSnip.Click
        GenMergedMaterialSnippet(NameHandleInputBox1.Text, tboxInputMaterialUUID.Text, tboxInputTexturePrefix.Text, tboxInputNMapUUID.Text, tboxInputBMapUUID.Text, MMapUUIDLabel.Text, tboxInputPMapUUID.Text)
        GenMergedTextureSnippet(NameHandleInputBox1.Text, tboxInputPMapUUID.Text, tboxInputTexturePrefix.Text, PMapPathBox.Text, tboxInputBMapUUID.Text, BMapPathBox.Text, tboxInputNMapUUID.Text, NMapPathBox.Text, tboxInputMMapUUID.Text, MMapPathBox.Text)
        GenMergedMeshSnippet(NameHandleInputBox1.Text, tboxInputMeshUUID.Text, tboxInputMeshPrefix.Text, GR2FilePathBox.Text, tboxInputMaterialUUID.Text)
    End Sub

    Private Sub multiGenButton_Click(sender As Object, e As EventArgs) Handles btnMultiGenSnips.Click 'I'm certain there's a less clunky way to do this but that's low priority right now.
        If MergedSnipGenCBox.Checked Then
            GenMergedMaterialSnippet(NameHandleInputBox1.Text, tboxInputMaterialUUID.Text, tboxInputTexturePrefix.Text, tboxInputNMapUUID.Text, tboxInputBMapUUID.Text, tboxInputMMapUUID.Text, tboxInputPMapUUID.Text)
            GenMergedTextureSnippet(NameHandleInputBox1.Text, tboxInputPMapUUID.Text, tboxInputTexturePrefix.Text, PMapPathBox.Text, tboxInputBMapUUID.Text, BMapPathBox.Text, tboxInputNMapUUID.Text, NMapPathBox.Text, tboxInputMMapUUID.Text, MMapPathBox.Text)
            GenMergedMeshSnippet(NameHandleInputBox1.Text, tboxInputMeshUUID.Text, tboxInputMeshPrefix.Text, GR2FilePathBox.Text, tboxInputMaterialUUID.Text)
        End If
        If LocaSnipGenCBox.Checked Then
            GenLocaSnip(NameHandleInputBox2.Text, DisplayedNameInputBox.Text)
        End If
        If CCAVIDGenCBox.Checked Then
            Dim bodyShape As String
            Dim bodyType As String
            Dim slot = cboxSlot.SelectedItem
            Dim race = cboxRace.SelectedItem

            If StandardBSRButton.Checked Then 'Standard
                bodyShape = "0"
            End If
            If StrongBSRButton.Checked Then 'Strong
                bodyShape = "1"
            End If

            If MascBTRButton.Checked Then 'Masc
                bodyType = "0"
            End If
            If FemBTRButton.Checked Then 'Fem
                bodyType = "1"
            End If

            GenCCAVSnippet(MeshUUIDInputBox2.Text, CCOptionUUIDInputBox.Text, NameHandleInputBox1.Text, bodyShape, bodyType, slot, race)
        End If
        If MetaSnipGenCBox.Checked Then
            GenMetaText(ModUUIDInputBox.Text, ModAuthorInputBox.Text, ModDescInputBox.Text, ModFolderInputBox.Text, ModNameInputBox.Text)
        End If
    End Sub

    Private Sub createMetaFileButton_Click(sender As Object, e As EventArgs) Handles btnCreateMetaFile.Click
        SaveFileDialog.Filter = "LSX Files (*lsx)|*.lsx"
        SaveFileDialog.FileName = "meta.lsx"
        If SaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Computer.FileSystem.WriteAllText(SaveFileDialog.FileName, MetaOutputRTBox.Text, True)
        End If
    End Sub

    Private Sub OneClickCopySettingsMenuItem_Click(sender As Object, e As EventArgs) Handles OneClickCopySettingsMenuItem.Click
        Dim isOptionChecked = OneClickCopySettingsMenuItem.Checked
        Select Case (isOptionChecked)
            Case "True"
                OneClickCopySettingsMenuItem.Checked = False
            Case "False"
                OneClickCopySettingsMenuItem.Checked = True
        End Select
    End Sub

    Private Sub MergedMaterialOutputBox_Click(sender As Object, e As EventArgs) Handles MergedMaterialOutputBox.Click
        If OneClickCopySettingsMenuItem.Checked Then
            My.Computer.Clipboard.SetText(MergedMaterialOutputBox.Text)
        End If
    End Sub

    Private Sub MetaOutputRTBox_Click(sender As Object, e As EventArgs) Handles MetaOutputRTBox.Click
        If OneClickCopySettingsMenuItem.Checked Then
            My.Computer.Clipboard.SetText(MetaOutputRTBox.Text)
        End If
    End Sub

    Private Sub LocaOutputBox_Click(sender As Object, e As EventArgs) Handles LocaOutputBox.Click
        If OneClickCopySettingsMenuItem.Checked Then
            My.Computer.Clipboard.SetText(LocaOutputBox.Text)
        End If
    End Sub

    Private Sub CCAVOutputBox_Click(sender As Object, e As EventArgs) Handles CCAVOutputBox.Click
        If OneClickCopySettingsMenuItem.Checked Then
            My.Computer.Clipboard.SetText(CCAVOutputBox.Text)
        End If
    End Sub

    Private Sub MergedTextureOutputBox_Click(sender As Object, e As EventArgs) Handles MergedTextureOutputBox.Click
        If OneClickCopySettingsMenuItem.Checked Then
            My.Computer.Clipboard.SetText(MergedTextureOutputBox.Text)
        End If
    End Sub

    Private Sub MergedMeshOutputBox_Click(sender As Object, e As EventArgs) Handles MergedMeshOutputBox.Click
        If OneClickCopySettingsMenuItem.Checked Then
            My.Computer.Clipboard.SetText(MergedMeshOutputBox.Text)
        End If
    End Sub

    Private Sub GenerateLocaFileMenuItem_Click(sender As Object, e As EventArgs) Handles GenerateLocaFileMenuItem.Click
        SaveFileDialog.Filter = "XML Files (*xml)|*.xml"
        Dim inputTemplate As String = My.Resources.SnippetResources.locaFileInput
        Dim inputSnippet As String = LocaOutputBox.Text
        Dim outputText As String = inputTemplate.Replace("LOCA_SNIPPET", inputSnippet)
        If SaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Computer.FileSystem.WriteAllText(SaveFileDialog.FileName, outputText, True)
        End If
    End Sub

    Private Sub GenerateCCAVFileMenuItem_Click(sender As Object, e As EventArgs) Handles GenerateCCAVFileMenuItem.Click
        SaveFileDialog.Filter = "LSX Files (*lsx)|*.lsx"
        SaveFileDialog.FileName = "CharacterCreationAppearanceVisuals.lsx"
        Dim inputTemplate As String = My.Resources.SnippetResources.CCAVFileInput
        Dim inputSnippet As String = CCAVOutputBox.Text
        Dim outputText As String = inputTemplate.Replace("CCAV_SNIPPET", inputSnippet)
        If SaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Computer.FileSystem.WriteAllText(SaveFileDialog.FileName, outputText, True)
        End If
    End Sub

    Private Sub GenerateMergedFileMenuItem_Click(sender As Object, e As EventArgs) Handles GenerateMergedFileMenuItem.Click
        SaveFileDialog.Filter = "LSX Files (*lsx)|*.lsx"
        SaveFileDialog.FileName = "_merged.lsx"
        Dim inputTemplate As String = My.Resources.SnippetResources.mergedFileInput
        Dim inputMaterialSnippet As String = MergedMaterialOutputBox.Text
        Dim inputTextureSnippet As String = MergedTextureOutputBox.Text
        Dim inputMeshSnippet As String = MergedMeshOutputBox.Text
        Dim replaceMaterial As String = inputTemplate.Replace("MERGED_MATERIAL_SNIPPET", inputMaterialSnippet)
        Dim replaceTexture As String = replaceMaterial.Replace("MERGED_TEXTURE_SNIPPET", inputTextureSnippet)
        Dim replaceMesh As String = replaceTexture.Replace("MERGED_MESH_SNIPPET", inputMeshSnippet)
        If SaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Computer.FileSystem.WriteAllText(SaveFileDialog.FileName, replaceMesh, True)
        End If
    End Sub

    Private Sub MaterialUUIDLockPBox_Click(sender As Object, e As EventArgs) Handles MaterialUUIDLockPBox.Click
        If MaterialUUIDLockPBox.Tag = "unlocked" Then
            MaterialUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.locked_icon
            MaterialUUIDLockPBox.Tag = "locked"
        Else
            MaterialUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            MaterialUUIDLockPBox.Tag = "unlocked"
        End If
    End Sub

    Private Sub PMapUUIDLockPBox_Click(sender As Object, e As EventArgs) Handles PMapUUIDLockPBox.Click
        If PMapUUIDLockPBox.Tag = "unlocked" Then
            PMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.locked_icon
            PMapUUIDLockPBox.Tag = "locked"
        Else
            PMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            PMapUUIDLockPBox.Tag = "unlocked"
        End If
    End Sub

    Private Sub BMapUUIDLockPBox_Click(sender As Object, e As EventArgs) Handles BMapUUIDLockPBox.Click
        If BMapUUIDLockPBox.Tag = "unlocked" Then
            BMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.locked_icon
            BMapUUIDLockPBox.Tag = "locked"
        Else
            BMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            BMapUUIDLockPBox.Tag = "unlocked"
        End If
    End Sub

    Private Sub NMapUUIDLockPBox_Click(sender As Object, e As EventArgs) Handles NMapUUIDLockPBox.Click
        If NMapUUIDLockPBox.Tag = "unlocked" Then
            NMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.locked_icon
            NMapUUIDLockPBox.Tag = "locked"
        Else
            NMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            NMapUUIDLockPBox.Tag = "unlocked"
        End If
    End Sub

    Private Sub MMapUUIDLockPBox_Click(sender As Object, e As EventArgs) Handles MMapUUIDLockPBox.Click
        If PMapUUIDLockPBox.Tag = "unlocked" Then
            MMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.locked_icon
            MMapUUIDLockPBox.Tag = "locked"
        Else
            MMapUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            MMapUUIDLockPBox.Tag = "unlocked"
        End If
    End Sub

    Private Sub CCOptionUUIDLockPBox_Click(sender As Object, e As EventArgs) Handles CCOptionUUIDLockPBox.Click
        If CCOptionUUIDLockPBox.Tag = "unlocked" Then
            CCOptionUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.locked_icon
            CCOptionUUIDLockPBox.Tag = "locked"
        Else
            CCOptionUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            CCOptionUUIDLockPBox.Tag = "unlocked"
        End If
    End Sub

    Private Sub ModUUIDLockPBox_Click(sender As Object, e As EventArgs) Handles ModUUIDLockPBox.Click
        If ModUUIDLockPBox.Tag = "unlocked" Then
            ModUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.locked_icon
            ModUUIDLockPBox.Tag = "locked"
        Else
            ModUUIDLockPBox.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            ModUUIDLockPBox.Tag = "unlocked"
        End If
    End Sub

    Private Sub NameHandleUUIDLockPBox1_Click(sender As Object, e As EventArgs) Handles NameHandleUUIDLockPBox1.Click
        If NameHandleUUIDLockPBox1.Tag = "unlocked" Then
            NameHandleUUIDLockPBox1.BackgroundImage = My.Resources.SnippetResources.locked_icon
            NameHandleUUIDLockPBox2.Tag = "locked"
            NameHandleUUIDLockPBox2.BackgroundImage = My.Resources.SnippetResources.locked_icon
            NameHandleUUIDLockPBox1.Tag = "locked"
        Else
            NameHandleUUIDLockPBox1.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            NameHandleUUIDLockPBox1.Tag = "unlocked"
            NameHandleUUIDLockPBox2.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            NameHandleUUIDLockPBox2.Tag = "unlocked"
        End If
    End Sub

    Private Sub NameHandleUUIDLockPBox2_Click(sender As Object, e As EventArgs) Handles NameHandleUUIDLockPBox2.Click
        If NameHandleUUIDLockPBox1.Tag = "unlocked" Then
            NameHandleUUIDLockPBox1.BackgroundImage = My.Resources.SnippetResources.locked_icon
            NameHandleUUIDLockPBox2.Tag = "locked"
            NameHandleUUIDLockPBox2.BackgroundImage = My.Resources.SnippetResources.locked_icon
            NameHandleUUIDLockPBox1.Tag = "locked"
        Else
            NameHandleUUIDLockPBox1.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            NameHandleUUIDLockPBox1.Tag = "unlocked"
            NameHandleUUIDLockPBox2.BackgroundImage = My.Resources.SnippetResources.unlocked_icon
            NameHandleUUIDLockPBox2.Tag = "unlocked"
        End If
    End Sub
End Class
