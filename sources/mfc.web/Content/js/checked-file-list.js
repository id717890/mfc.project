function add_checked_file(file_id) {
    init_checked_files_storage();

    var checkboxes = get_checked_files();
    checkboxes[file_id] = true;
    save_checked_files(checkboxes);
}

function remove_checked_file(file_id) {
    init_checked_files_storage();

    var checkboxes = get_checked_files();
    checkboxes[file_id] = false;
    save_checked_files(checkboxes);
}

function get_checked_files() {
    init_checked_files_storage();

    return JSON.parse(localStorage.getItem("checkedFiles"));
}

function clear_checked_files() {
    localStorage.setItem("checkedFiles", JSON.stringify({}));
}

function init_checked_files_storage() {
    if (localStorage.getItem("checkedFiles") == "") {
        localStorage.setItem("checkedFiles", JSON.stringify({}));
        console.log("checkedFiles local storage created");
    }
}

function save_checked_files(files) {
    localStorage.setItem("checkedFiles", JSON.stringify(files));
}